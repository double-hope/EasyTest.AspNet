using AutoMapper;
using EasyTest.BLL.Mappers;
using EasyTest.BLL.Services;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.DTO.Test;
using FakeItEasy;

namespace EasyTest.BLL.Tests.Services
{
    public class TestServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TestMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task GetAll_ReturnsAllTests()
        {
            // Arrange
            var testEntities = A.Fake<List<Test>>();
            A.CallTo(() => _unitOfWork.TestRepository.GetAll()).Returns(testEntities);
            var testService = new TestService(_unitOfWork, _mapper);

            // Act
            var result = await testService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntities.Count, result.Data.Count());
        }

        [Fact]
        public async Task Get_ReturnsTestById()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var testEntity = new Test { Id = testId, Title = "Title", Description = "Description" };
            A.CallTo(() => _unitOfWork.TestRepository.GetById(testId)).Returns(testEntity);
            var testService = new TestService(_unitOfWork, _mapper);

            // Act
            var result = await testService.Get(testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntity.Title, result.Data.Title);
        }

        [Fact]
        public async Task Create_ReturnsCreatedTest()
        {
            // Arrange
            var testCreateDto = new TestCreateDto { Title = "Title", Description = "Description" };
            var testEntity = _mapper.Map<Test>(testCreateDto);

            A.CallTo(() => _unitOfWork.TestRepository.Add(A<Test>.Ignored))
                .Invokes((Test addedTest) => testEntity = addedTest);

            var testService = new TestService(_unitOfWork, _mapper);

            // Act
            var result = await testService.Create(testCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntity.Title, result.Data.Title);
            Assert.Equal("Test created successfully", result.Message);
        }
        [Fact]
        public async Task Edit_ReturnsUpdatedTest()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var testEditDto = new TestEditDto { Title = "Title2", Description = "Description2" };
            var testEntity = _mapper.Map<Test>(testEditDto);

            A.CallTo(() => _unitOfWork.TestRepository.Update(A<Test>.Ignored));
            A.CallTo(() => _unitOfWork.Save());

            var testService = new TestService(_unitOfWork, _mapper);

            // Act
            var result = await testService.Edit(testId, testEditDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntity.Title, result.Data.Title);
            Assert.Equal("Test updated successfully", result.Message);
        }
    }
}
