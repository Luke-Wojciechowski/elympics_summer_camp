﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Secs.Debug 
{
    [CustomEditor(typeof(EcsEntityObserver))]
    public sealed class EcsEntityObserverInspector : Editor
    {
        private const float REFRESH_RATE = 0.2f;
        private static int _maximumNumberOfComponents = 32;
        
        private Type[] _cashedComponentTypes = new Type[_maximumNumberOfComponents];
        private object[] _cashedComponents = new object[_maximumNumberOfComponents];
        private int _numberOfComponents;
        
        private EcsEntityObserver _entityObserver;
        private int _entityId = -1;
        private EcsWorld _ecsWorld;

        private readonly Dictionary<string, Type> _popupStringToTypeDiction = new();
        private IEnumerable<Type> _cmpTypes; 
        private int _popupIndex;
        private string[] _popupOptions;
        private object _popupObject;

        private float _refreshTimer = 0.3f;
        private bool _isEntityDead;
        private void Awake()
        {
            var list = new List<string>();
            
            _cmpTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => e.GetInterfaces()
                    .Contains(typeof(IEcsComponent)));
            
            foreach (var cmpType in _cmpTypes)
            {
                list.Add(cmpType.Name);
                _popupStringToTypeDiction.Add(cmpType.Name, cmpType);
            }

            _popupOptions = list.ToArray();
        }

        void OnEnable()
        {
            _entityObserver = target as EcsEntityObserver;
            
            if (_entityObserver == null) 
                return;
            
            _entityId = _entityObserver.entityId;
            _ecsWorld= _entityObserver.world;

            if (_entityId == -1 || _ecsWorld == null)
                return;
            
            InitComponents();
            
            var cmpType = _popupStringToTypeDiction[_popupOptions[_popupIndex]];
            _popupObject = Activator.CreateInstance(cmpType);

            _refreshTimer = 0f;
            EditorApplication.update += OnUpdate;
            _ecsWorld.OnComponentAddedToEntity += OnComponentAddedToEntity;
            _ecsWorld.OnComponentDeletedFromEntity += OnComponentDeletedToEntity;
        }

        private void OnDisable()
        {
            if(_ecsWorld != null)
            {
                _ecsWorld.OnComponentAddedToEntity -= OnComponentAddedToEntity;
                _ecsWorld.OnComponentDeletedFromEntity -= OnComponentDeletedToEntity;
            }
            
            _entityObserver = null;
            _entityId = -1;
            _ecsWorld = null;
            _popupObject = null;
            
            EditorApplication.update -= OnUpdate;
        }
        
        private void OnUpdate()
        {
            _refreshTimer += Time.deltaTime;
            
            if(_refreshTimer < REFRESH_RATE)
                return;
            
            _refreshTimer = 0f;

            if (!_ecsWorld.AliveEntities.Contains(_entityId))
            {
                _isEntityDead = true;
                return;
            }

            _isEntityDead = false;
            
            var shouldRepaint = false;
            for (int i = 0; i < _numberOfComponents; i++)
            {
                ref var componentValue = ref _cashedComponents[i];
                
                var result = _ecsWorld
                    .GetType()
                    .GetMethod(nameof(EcsWorld.IsSame),BindingFlags.NonPublic | BindingFlags.Instance)?
                    .MakeGenericMethod(_cashedComponentTypes[i])
                    .Invoke(_ecsWorld, new [] { _entityId, componentValue});
                
                if (result != null && (shouldRepaint = !(bool)result))
                {
                    var typeValue = _ecsWorld
                        .GetType()
                        .GetMethod(nameof(EcsWorld.GetItem),BindingFlags.NonPublic | BindingFlags.Instance)?
                        .MakeGenericMethod(_cashedComponentTypes[i])
                        .Invoke(_ecsWorld, new object[] { _entityId,});

                    _cashedComponents[i] = typeValue;
                }

                if (!shouldRepaint) 
                    continue;
                
                Repaint();
                return;
            }
        }
        public override void OnInspectorGUI()
        {
            if (_entityObserver == null) 
                return;
            
            serializedObject.Update();
            

            if (_entityId == -1 || _ecsWorld == null)
            {
                EditorGUILayout.LabelField("The observer does not observe any entity");
                return;
            }

            if (_isEntityDead)
            {
                EditorGUILayout.LabelField("The entity is dead");
                return;
            }
            
            CreateAddComponentScope();

            for (int i = 0; i < _numberOfComponents; i++)
            {
                GUI.backgroundColor = EntityColorPallet.GetColorByIndex(i);
                
                var type = _cashedComponentTypes[i];
                
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    if(CreateRemoveComponentScope(type))
                        return;
                    
                    CreateViewComponentScope(type, i);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnComponentAddedToEntity(int entity, Type type)
        {
            if(entity != this._entityId)
                return;

            var typeValue = _ecsWorld
                .GetType()
                .GetMethod(nameof(EcsWorld.GetItem),BindingFlags.NonPublic | BindingFlags.Instance)?
                .MakeGenericMethod(type)
                .Invoke(_ecsWorld, new object[] { _entityId,});
            
            if (typeValue == null) 
                return;
            
            AdjustBufferSize();
            
            _cashedComponents[_numberOfComponents] = typeValue;
            _cashedComponentTypes[_numberOfComponents] = type;
            _numberOfComponents++;
    
            Repaint();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnComponentDeletedToEntity(int entity, Type type)
        {
            if(entity != this._entityId)
                return;
            
            InitComponents();
            Repaint();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InitComponents()
        {
            if (!_ecsWorld.AliveEntities.Contains(_entityId))
            {
                _isEntityDead = true;
                return;
            }

            _isEntityDead = false;
            
            _numberOfComponents = 0;
            var types = _ecsWorld.GetEntityComponentsTypeMask(_entityId).GetComponents();
            
            foreach (var type in types)
            {
                var typeValue = _ecsWorld
                    .GetType()
                    .GetMethod(nameof(EcsWorld.GetItem),BindingFlags.NonPublic | BindingFlags.Instance)?
                    .MakeGenericMethod(type)
                    .Invoke(_ecsWorld, new object[] { _entityId,});
                
                if (typeValue == null) 
                    continue;

                AdjustBufferSize();
                
                _cashedComponents[_numberOfComponents] = typeValue;
                _cashedComponentTypes[_numberOfComponents] = type;
                _numberOfComponents++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AdjustBufferSize()
        {
            if (_numberOfComponents < _maximumNumberOfComponents) 
                return;
            
            _maximumNumberOfComponents += _maximumNumberOfComponents / 2;
                        
            var cashedComponents = _cashedComponents;
            var cashedComponentTypes = _cashedComponentTypes;

            _cashedComponents = new object[_maximumNumberOfComponents];
            _cashedComponentTypes = new Type[_maximumNumberOfComponents];
                        
            cashedComponents.CopyTo(_cashedComponents,0);
            cashedComponentTypes.CopyTo(_cashedComponentTypes,0);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateAddComponentScope()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using var popupChange = new EditorGUI.ChangeCheckScope();
                _popupIndex = EditorGUILayout.Popup("Components:", _popupIndex, _popupOptions);
                var cmpType = _popupStringToTypeDiction[_popupOptions[_popupIndex]];
                
                if (popupChange.changed)
                    _popupObject = Activator.CreateInstance(cmpType);
                
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    var cashedFields = cmpType.GetFields();
                    foreach (var field in cashedFields)
                    {
                        using var fieldChange = new EditorGUI.ChangeCheckScope();
                        var newValue = EcsComponentDrawer.Draw(field.FieldType, field.Name, field.GetValue(_popupObject),1);
                        if (!fieldChange.changed)
                            continue;
                        
                        field.SetValue(_popupObject, newValue);
                    }

                    if (GUILayout.Button("Add"))
                    {
                        _ecsWorld
                            .GetType()
                            .GetMethod(nameof(EcsWorld.AddItem), BindingFlags.NonPublic | BindingFlags.Instance)?
                            .MakeGenericMethod(cmpType)
                            .Invoke(_ecsWorld, new []
                                {
                                    _entityId, 
                                    _popupObject
                                }
                            );
                        
                        InitComponents();
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool CreateRemoveComponentScope(in Type type)
        {
            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField($"{type.Name}");
                if (GUILayout.Button("-"))
                {
                    _ecsWorld
                        .GetType()
                        .GetMethod(nameof(EcsWorld.DeleteComponent),BindingFlags.NonPublic | BindingFlags.Instance)?
                        .MakeGenericMethod(type)
                        .Invoke(_ecsWorld, new object[]
                            {
                                _entityId
                            }
                        );
                            
                    InitComponents();
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateViewComponentScope(in Type type, in int componentId)
        {
            var cashedFields = type.GetFields();
            foreach (var field in cashedFields)
            {
                using (new EditorGUILayout.HorizontalScope("box"))
                {
                    using (var change = new EditorGUI.ChangeCheckScope())
                    {
                        var newValue = EcsComponentDrawer.Draw(field.FieldType, field.Name, field.GetValue(_cashedComponents[componentId]),1);
                        if (!change.changed)
                            continue;

                        field.SetValue(_cashedComponents[componentId], newValue);
                        _ecsWorld
                            .GetType()
                            .GetMethod(nameof(EcsWorld.ReplaceItem), BindingFlags.NonPublic | BindingFlags.Instance)?
                            .MakeGenericMethod(type)
                            .Invoke(_ecsWorld, new []
                                {
                                    _entityId, 
                                    _cashedComponents[componentId]
                                }
                            );
                    }
                }
            }
        }
    }
}
#endif