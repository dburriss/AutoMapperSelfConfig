using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMapperSelfConfig.Core
{
    public class MappingLoader
    {
        public static void LoadAllMappings(IEnumerable<Type> types)
        {
            LoadStandardMappings(types);
            LoadCustomMappings(types);
        }

        public static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var instancesToMap = (from t in types
                        from i in GetInterfaces(t)
                        where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                              !IsAbstract(t) &&
                              !IsInterface(t)
                        select InitializeCustomMappingObject(t)).ToArray();

            foreach (var map in instancesToMap)
            {
                map.CreateMappings(Mapper.Configuration);
            }
        }

        private static IEnumerable<Type> GetInterfaces(Type type)
        {
#if DOTNET5_4 || DNXCORE50
            return type.GetTypeInfo().ImplementedInterfaces;
#endif
#if NET46 || NET452 || NET451 || DNX46 || DNX452 || DNX451
            return type.GetInterfaces().AsEnumerable();
#endif
            throw new NotImplementedException();
        }

        private static bool IsAbstract(Type type)
        {
#if DOTNET5_4 || DNXCORE50
            return type.GetTypeInfo().IsAbstract;
#endif
#if NET46 || NET452 || NET451 || DNX46 || DNX452 || DNX451
            return type.IsAbstract;
#endif
            throw new NotImplementedException();
        }

        private static bool IsInterface(Type type)
        {
#if DOTNET5_4 || DNXCORE50
            return type.GetTypeInfo().IsInterface;
#endif
#if NET46 || NET452 || NET451 || DNX46 || DNX452 || DNX451
            return type.IsInterface;
#endif
            throw new NotImplementedException();
        }

        private static bool IsGenericType(Type type)
        {
#if DOTNET5_4 || DNXCORE50
            return type.GetTypeInfo().IsGenericType;
#endif
#if NET46 || NET452 || NET451 || DNX46 || DNX452 || DNX451
            return type.IsGenericType;
#endif
            throw new NotImplementedException();
        }

        private static IHaveCustomMappings InitializeCustomMappingObject(Type t)
        {
            return (IHaveCustomMappings)Activator.CreateInstance(t, true);
        }

        public static void LoadStandardMappings(IEnumerable<Type> types)
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
                Mapper.CreateMap(map.Source, map.Destination);
            }

            var mapsTo = (from t in types
                          from i in t.GetInterfaces()
                          where IsGenericType(i)&& i.GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                                !IsAbstract(t) &&
                                !IsInterface(t)
                          select new
                          {
                              Source = t,
                              Destination = i.GetGenericArguments()[0]
                          }).ToArray();

            foreach (var map in mapsTo)
            {
                Mapper.CreateMap(map.Source, map.Destination);
            }
        }
    }
}
