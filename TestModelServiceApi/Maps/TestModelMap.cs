using ApiIntegrationTests.DTOs;
using ApiIntegrationTests.Models;
using AutoMapper;

namespace ApiIntegrationTests.Maps
{
    public static class TestModelMap
    {
        private static Mapper? _instance;

        public static Mapper Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new Mapper(
                        new MapperConfiguration(cfg =>
                            cfg.CreateMap<TestModel, TestModelDTO>()
                            .ReverseMap()
                    ));
                }
                return _instance;
            }

            private set { _instance = value; }
        }
    }
}