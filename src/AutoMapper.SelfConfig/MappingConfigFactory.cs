using System;
using System.Collections.Generic;
using System.Linq;
using static PhilosophicalMonkey.Reflect.OnTypes;

namespace AutoMapper.SelfConfig
{
    public class MappingConfigFactory
    {
        public static MapperConfiguration CreateConfiguration(IList<Type> types)
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                LoadAllMappings(cfg, types);
            });

            return config;
        }

        public static void LoadAllMappings(IList<Type> types)
        {
            Mapper.Initialize(
                cfg =>
                {
                    LoadStandardMappings(cfg, types);
                    LoadCustomMappings(cfg, types);
                });
        }

        public static void LoadAllMappings(IMapperConfigurationExpression config, IList<Type> types)
        {
            LoadStandardMappings(config, types);
            LoadCustomMappings(config, types);
        }

        public static void LoadCustomMappings(IMapperConfigurationExpression config, IList<Type> types)
        {
            var instancesToMap = (from t in types
                                  from i in GetInterfaces(t)
                                  where IsAssignable(t, typeof(IHaveCustomMappings)) &&
                                        !IsAbstract(t) &&
                                        !IsInterface(t)
                                  select InitializeCustomMappingObject(t)).ToArray();

            foreach (var map in instancesToMap)
            {
                map.CreateMappings(config);
            }
        }
        
        private static IHaveCustomMappings InitializeCustomMappingObject(Type t)
        {
            return (IHaveCustomMappings)Activator.CreateInstance(t, true);
        }

        public static void LoadStandardMappings(IMapperConfigurationExpression config, IList<Type> types)
        {
            var mapsFrom = (from t in types
                            from i in GetInterfaces(t)
                            where IsGenericType(i) && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                  !IsAbstract(t) &&
                                  !IsInterface(t)
                            select new
                            {
                                Source = GetGenericArguments(i).First(),
                                Destination = t
                            }).ToArray();

            foreach (var map in mapsFrom)
            {
                config.CreateMap(map.Source, map.Destination);
            }

            var mapsTo = (from t in types
                          from i in GetInterfaces(t)
                          where IsGenericType(i) && i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                                !IsAbstract(t) &&
                                !IsInterface(t)
                          select new
                          {
                              Source = t,
                              Destination = GetGenericArguments(i).First()
                          }).ToArray();

            foreach (var map in mapsTo)
            {
                config.CreateMap(map.Source, map.Destination);
            }

        }
    }
}