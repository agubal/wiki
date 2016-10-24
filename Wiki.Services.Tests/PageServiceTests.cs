using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wiki.Data;
using Wiki.Entities.Domain;
using Wiki.Entities.Events;
using Wiki.Services.Events;
using Wiki.Services.Pages;

namespace Wiki.Services.Tests
{
    [TestClass]
    public class PageServiceTests
    {
        private Mock<IMediator> _mediatrMock;
        private Mock<IEventService> _eventServiceMock;
        private Mock<IRepository> _repositoryMock;
        private IPageService _pageService;

        [TestInitialize]
        public void Initialize()
        {
            _mediatrMock = new Mock<IMediator>();
            _eventServiceMock = new Mock<IEventService>();
            _repositoryMock = new Mock<IRepository>();
            _pageService = new PageService(_mediatrMock.Object, _eventServiceMock.Object);
        }

        [TestMethod]
        public void CreateAsync_PositiveTest()
        {
            //Arrange:
            var page = new Page
            {
                Title = "Title",
                Text = "Text"
            };
            _mediatrMock.Setup(g => g.SendAsync(It.IsAny<IAsyncRequest>())).ReturnsAsync(It.IsAny<Unit>());

            //Act:
            var serviceResult = _pageService.CreateAsync(page).Result;

            //Assert:
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);
        }

        [TestMethod]
        public void UpdateAsync_PositiveTest()
        {
            //Arrange:
            var page = new Page
            {
                Id = 1,
                Title = "Title",
                Text = "Text"
            };
            _mediatrMock.Setup(g => g.SendAsync(It.IsAny<IAsyncRequest<Page>>())).ReturnsAsync(page);
            _eventServiceMock.Setup(g => g.CreateEntityEventAsync(page)).Returns(Task.CompletedTask);
            //Act:
            var serviceResult = _pageService.CreateAsync(page).Result;

            //Assert:
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);
        }

        [TestMethod]
        public void GetAll_PositiveTest()
        {
            //Arrange:
            var pages = new List<Page>();
            const int numberOfPages = 5;
            for (int i = 1; i <= numberOfPages; i++)
            {
                var page = new Page {Id = i, Title = "Title" + i, Text = "Text" + i};
                pages.Add(page);

                var pageEvent = new Event
                {
                    Version = 1,
                    AggregateId = i,
                    Id = 1,
                    Timestamp = DateTime.UtcNow,
                    Data = "{" +
                           $"'Title':'Title{i}{i}','Text':'Text{i}{i}'" +
                           "}"
                };
                var pageEvents = new List<Event> {pageEvent};
                _eventServiceMock.Setup(g=>g.GetEventsByAggregateIdAsync(page.Id)).ReturnsAsync(pageEvents.AsEnumerable());
            }         
            _mediatrMock.Setup(g=>g.SendAsync(It.IsAny<IAsyncRequest<IEnumerable<Page>>>())).ReturnsAsync( pages.AsEnumerable());

            //Act:
            var serviceResult = _pageService.GetAllAsync().Result;

            //Assert:
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);
            Assert.AreEqual(numberOfPages, serviceResult.Result.Count());
            foreach (var page in serviceResult.Result)
            {
                Assert.IsNotNull(page);
                Assert.AreEqual($"Title{page.Id}{page.Id}", page.Title);
                Assert.AreEqual($"Text{page.Id}{page.Id}", page.Text);
            }

        }

        [TestMethod]
        public void GetByIdAsync_CurrentVersion_PositiveTest()
        {
            //Arrange:
            var page = new Page { Id = 1, Title = "Title", Text = "Text"};
            const int numberOfPages = 5;
            int currentVersion = 1;
            var pageEvents = new List<Event>();
            for (int i = 1; i <= numberOfPages; i++)
            {
                var pageEvent = new Event
                {
                    Version = currentVersion++,
                    AggregateId = page.Id,
                    Id = 1,
                    Timestamp = DateTime.UtcNow.AddHours(numberOfPages - currentVersion),
                    Data = "{" +
                           $"'Title':'Title{i}','Text':'Text{i}'" +
                           "}"
                };
                pageEvents.Add(pageEvent);
            }
            _eventServiceMock.Setup(g => g.GetEventsByAggregateIdAsync(page.Id)).ReturnsAsync(pageEvents.AsEnumerable());
            _mediatrMock.Setup(g => g.SendAsync(It.IsAny<IAsyncRequest<Page>>())).ReturnsAsync(page);

            //Act:
            var serviceResult = _pageService.GetByIdAsync(page.Id, null).Result;

            //Assert:
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);
            Assert.AreEqual($"Title{numberOfPages}", page.Title);
            Assert.AreEqual($"Text{numberOfPages}", page.Text);
        }


        [TestMethod]
        public void GetByIdAsync_SpecificVersion_PositiveTest()
        {
            //Arrange:
            var page = new Page { Id = 1, Title = "Title", Text = "Text" };
            const int numberOfPages = 5;
            const int versionToReturn = 3;
            int currentVersion = 1;
            var pageEvents = new List<Event>();
            for (int i = 1; i <= numberOfPages; i++)
            {
                var pageEvent = new Event
                {
                    Version = currentVersion++,
                    AggregateId = page.Id,
                    Id = 1,
                    Timestamp = DateTime.UtcNow.AddHours(numberOfPages - currentVersion),
                    Data = "{" +
                           $"'Title':'Title{i}','Text':'Text{i}'" +
                           "}"
                };
                pageEvents.Add(pageEvent);          
            }
            _eventServiceMock.Setup(g => g.GetEventsByAggregateIdAsync(page.Id)).ReturnsAsync(pageEvents.AsEnumerable());
            _mediatrMock.Setup(g => g.SendAsync(It.IsAny<IAsyncRequest<Page>>())).ReturnsAsync(page);

            //Act:
            var serviceResult = _pageService.GetByIdAsync(page.Id, versionToReturn).Result;

            //Assert:
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);
            Assert.AreEqual($"Title{versionToReturn}", page.Title);
            Assert.AreEqual($"Text{versionToReturn}", page.Text);
        }


        [TestMethod]
        public void GetAllPageVersions_PositiveTest()
        {
            //Arrange:
            var page = new Page { Id = 1, Title = "Title", Text = "Text" };
            const int numberOfPages = 5;
            int currentVersion = 1;
            var pageEvents = new List<Event>();
            for (int i = 1; i <= numberOfPages; i++)
            {
                var title = "Title" + i;
                var text = "Text" + i;

                var pageEvent = new Event
                {
                    Version = currentVersion++,
                    AggregateId = page.Id,
                    Id = i,
                    Timestamp = DateTime.UtcNow.AddHours(numberOfPages - currentVersion),
                    Data = "{" +
                           $"'Title':'{title}','Text':'{text}'" +
                           "}"
                };
                pageEvents.Add(pageEvent);

                var versionedPage = new Page { Id = page.Id, Title = title, Text = text };
                _eventServiceMock.Setup(g => g.GetPageFromEvent(pageEvent)).Returns(versionedPage);
            }
            _eventServiceMock.Setup(g => g.GetEventsByAggregateIdAsync(page.Id)).ReturnsAsync(pageEvents.AsEnumerable());
            _mediatrMock.Setup(g => g.SendAsync(It.IsAny<IAsyncRequest<Page>>())).ReturnsAsync(page);

            //Act:
            var serviceResult = _pageService.GetAllPageVersionsAsync(page.Id).Result;

            //Assert:
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);
            Assert.IsNotNull(page);
            Assert.AreEqual(numberOfPages + 1, serviceResult.Result.Count());
            Assert.IsTrue(serviceResult.Result.Any(r=>r.Title.Equals(page.Title) && r.Text.Equals(page.Text)));

            for (int i = 1; i < numberOfPages; i++)
            {
                var title = "Title" + i;
                var text = "Text" + i;
                Assert.IsTrue(serviceResult.Result.Any(r => r.Title.Equals(title) && r.Text.Equals(text)));
            }
        }

    }
}
