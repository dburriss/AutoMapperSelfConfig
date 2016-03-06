using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMapper.SelfConfig
{
    public class MappingConfigFactory
    {
        public static MapperConfiguration CreateConfiguration(IEnumerable<Type> types)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                LoadAllMappings(cfg, types);
            });

            return config;
        }

        public static void LoadAllMappings(IEnumerable<Type> types)
        {
            Mapper.Initialize(
                cfg =>
                {
                    LoadStandardMappings(cfg, types);
                    LoadCustomMappings(cfg, types);
                });
        }

        public static void LoadAllMappings(IMapperConfiguration config, IEnumerable<Type> types)
        {
            LoadStandardMappings(config, types);
            LoadCustomMappings(config, types);
        }

        public static void LoadCustomMappings(IMapperConfiguration config, IEnumerable<Type> types)
        {
            var instancesToMap = (from t in types
                                  from i in GetInterfaces(t)
                                  where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                                        !IsAbstract(t) &&
                                        !IsInterface(t)
                                  select InitializeCustomMappingObject(t)).ToArray();

            foreach (var map in instancesToMap)
            {
                map.CreateMappings(config);
            }
        }

        private static IEnumerable<Type> GetInterfaces(Type type)
        {
#if COREFX
            return type.GetTypeInfo().ImplementedInterfaces;
#endif
#if NET
            return type.GetInterfaces().AsEnumerable();
#endif
            throw new NotImplementedException();
        }

        private static bool IsAbstract(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsAbstract;
#endif
#if NET
            return type.IsAbstract;
#endif
            throw new NotImplementedException();
        }

        private static bool IsInterface(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsInterface;
#endif
#if NET
            return type.IsInterface;
#endif
            throw new NotImplementedException();
        }

        private static bool IsGenericType(Type type)
        {
#if COREFX
            return type.GetTypeInfo().IsGenericType;
#endif
#if NET
            return type.IsGenericType;
#endif
            throw new NotImplementedException();
        }

        private static IHaveCustomMappings InitializeCustomMappingObject(Type t)
        {
            return (IHaveCustomMappings)Activator.CreateInstance(t, true);
        }

        public static void LoadStandardMappings(IMapperConfiguration config, IEnumerable<Type> types)
        {
            var mapsFrom = (from t in types
                            from i in t.GetInterfaces()
                            where IsGenericType(i) && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                  !IsAbstract(t) &&
                                  !IsInterface(t)
                            select new
                            {
                                Source = i.GetGenericArguments()[0],
                                Destination = t
                            }).ToArray();

            foreach (var map in mapsFrom)
            {
                config.CreateMap(map.Source, map.Destination);
            }

            var mapsTo = (from t in types
                          from i in t.GetInterfaces()
                          where IsGenericType(i) && i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                                !IsAbstract(t) &&
                                !IsInterface(t)
                          select new
                          {
                              Source = t,
                              Destination = i.GetGenericArguments()[0]
                          }).ToArray();

            foreach (var map in mapsTo)
            {
                config.CreateMap(map.Source, map.Destination);
            }

        }
    }
}