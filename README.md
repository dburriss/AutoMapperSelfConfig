# AutoMapper Self Config
I help create mappings for AutoMapper based on interfaces.

Based on code presented on Pluralsight by @matthoneycutt. He blogs at http://trycatchfail.com/blog.


#Usage
The library provides 3 interfaces and some helper methods.

Install with the following nuget command:
> `Install-Package AutoMapper.SelfConfig -Pre`

## IMapFrom<T>
    
    public interface IMapFrom<T>
	{}
    
This is placed on a class to mark that an AutoMapper configuration be created mapping *from* T to the current class.

## IMapTo<T>
    
    public interface IMapTo<T>
	{}
    
This is the reverse of IMapFrom. This is placed on a class to mark that an AutoMapper configuration be created mapping *to* T from the current class.

The advantage with the 2 way mapping is you can place all mappings on say your **view models** and leave your ** domain models** free of mapping clutter while still keeping them close at hand with the files you working on rather than off in a seperate mapping Profile.

## IHaveCustomMapping

    public interface IHaveCustomMappings
	{
		void CreateMappings(IConfiguration configuration);
	}
    
This is for when more complex mappings are required and allows you to directly interact with the [AutoMapper IConfiguration](https://github.com/AutoMapper/AutoMapper/wiki/Configuration).


### Example Usage

    public class Source
    {
        public int Nr { get; set; }
        public string Name { get; set; }
    }
    
    public class TestEasy : IMapTo<Source>, IMapFrom<Source>
    {
        public int Nr { get; set; }
        public string Name { get; set; }
    }
    
Then the mappings are created as such:
    
    var types = new Type[] { typeof(TestEasy) };
    MappingLoader.LoadStandardMappings(types);

This would create a 2 mappings in AutoMapper *TestEasy -> Source* and *Source -> TestEasy*.

If you need finer control of the mappings above AutoMappers defaults you can use IHaveCustomMApping interface:

    public class TestConfig : IHaveCustomMappings
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Source, TestConfig>().ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Nr));
            configuration.CreateMap<TestConfig, Source>().ForMember(dest => dest.Nr, opt => opt.MapFrom(src => src.Number)); ;
        }
    }
    
Loading both the simple and complex maps is now as simple as:

    var types = new Type[] { typeof(TestEasy), typeof(TestConfig) };
    MappingLoader.LoadAllMappings(types);

## Real World Example

The command class below is taken from a real project and uses the [My Reflection Library](https://github.com/dburriss/PhilosophicalMonkey) and [My ASP.NET Lifecycle Middleware](https://github.com/dburriss/AspNetLifecycle) To load up all mappings at startup of a ASP.NET application.

    public class AutoMapperSetupTask : IRunAtStartup
    {
        public void Execute()
        {
            var seedTypes = new Type[] { typeof(Startup) };
            var assemblies = Reflect.OnTypes.GetAssemblies(seedTypes);
            var typesInAssemblies = Reflect.OnTypes.GetAllExportedTypes(assemblies);
            AutoMapper.SelfConfig.MappingLoader.LoadAllMappings(typesInAssemblies);
        }
    }