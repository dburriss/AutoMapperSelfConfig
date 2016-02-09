using AutoMapper;
using AutoMapperSelfConfig.Core.Tests.Models;
using System;
using Xunit;

namespace AutoMapperSelfConfig.Core.Tests
{
    public class MappingLoaderTests
    {
        public MappingLoaderTests()
        {
            Mapper.Reset();
        }

        [Fact]
        public void LoadStandardMappings_LoadsMappings_IMapToAndIMapFromRegistered()
        {
            var types = new Type[] { typeof(TestEasy) };
            MappingLoader.LoadStandardMappings(types);
            var mappings = Mapper.GetAllTypeMaps();
            Assert.True(mappings.Length == 2);
        }

        [Fact]
        public void LoadCustomMappings_LoadsMappings_CreateMappingsCalled()
        {
            var types = new Type[] { typeof(TestConfig) };
            MappingLoader.LoadCustomMappings(types);
            var mappings = Mapper.GetAllTypeMaps();
            Assert.True(mappings.Length == 2);
        }

        [Fact]
        public void LoadAllMappings_LoadsMappings_BothStandardAndCustom()
        {
            var types = new Type[] { typeof(TestEasy), typeof(TestConfig) };
            MappingLoader.LoadAllMappings(types);
            var mappings = Mapper.GetAllTypeMaps();
            Assert.True(mappings.Length  == 4);
        }
    }
}
