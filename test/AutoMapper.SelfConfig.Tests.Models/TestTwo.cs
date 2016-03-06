namespace AutoMapper.SelfConfig.Tests.Models
{
    public class TestToTwo : IMapTo<TestConfig>, IMapTo<TestEasy>
    {
    }

    public class TestFromTwo : IMapFrom<TestConfig>, IMapFrom<TestEasy>
    {
    }
}
