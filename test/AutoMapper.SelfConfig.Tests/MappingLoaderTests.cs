using AutoMapper.SelfConfig.Tests.Models;
using System;
using Xunit;

namespace AutoMapper.SelfConfig.Tests
{
    public class MappingLoaderTests
    {
        [Fact]
        public void LoadStandardMappings_LoadsMappings_IMapToAndIMapFromRegistered()
        {
            var types = new Type[] { typeof(TestEasy) };
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {                
                MappingConfigFactory.LoadStandardMappings(cfg, types);
            });
            var mappings = config.CreateMapper().ConfigurationProvider.GetAllTypeMaps();
            Assert.Equal(2, mappings.Length);
        }

        [Fact]
        public void LoadCustomMappings_LoadsMappings_CreateMappingsCalled()
        {
            var types = new Type[] { typeof(TestConfig) };
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                MappingConfigFactory.LoadCustomMappings(cfg, types);
            });
            var mappings = config.CreateMapper().ConfigurationProvider.GetAllTypeMaps();
            Assert.True(mappings.Length == 2);
        }

        [Fact]
        public void LoadAllMappings_LoadsMappings_BothStandardAndCustom()
        {
            var types = new Type[] { typeof(TestEasy), typeof(TestConfig) };
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                MappingConfigFactory.LoadAllMappings(cfg, types);
            });
            var mappings = config.CreateMapper().ConfigurationProvider.GetAllTypeMaps();
            Assert.Equal(4, mappings.Length);
        }

        [Fact]
        public void LoadAllMappings_With2MapTo_MapsBoth()
        {
            var types = new Type[] { typeof(TestToTwo) };
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                MappingConfigFactory.LoadAllMappings(cfg, types);
            });
            var mappings = config.CreateMapper().ConfigurationProvider.GetAllTypeMaps();
            Assert.True(mappings.Length == 2);
        }

        [Fact]
        public void LoadAllMappings_With2MapFrom_MapsBoth()
        {
            var types = new Type[] { typeof(TestFromTwo) };
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                MappingConfigFactory.LoadAllMappings(cfg, types);
            });
            var mappings = config.CreateMapper().ConfigurationProvider.GetAllTypeMaps();
            Assert.True(mappings.Length == 2);
        }

        [Fact]
        public void LoadAllMappings_WithNoMappings_MapsNone()
        {
            var types = new Type[] { typeof(Source) };
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                MappingConfigFactory.LoadAllMappings(cfg, types);
            });
            var mappings = config.CreateMapper().ConfigurationProvider.GetAllTypeMaps();
            Assert.True(mappings.Length == 0);
        }
    }
}
