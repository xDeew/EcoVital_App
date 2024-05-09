using EcoVital.ViewModels;

namespace UnitTestsEcoVital
{
    public class HomePageViewModelTests
    {
        private readonly HomePageViewModel _viewModel;

        public HomePageViewModelTests()
        {
            _viewModel = new HomePageViewModel();
        }

        [Fact]
        public void UserName_DefaultsToPredefinedValue()
        {
            // Assert
            Assert.Equal("usuario predeterminado", _viewModel.UserName);
        }

        [Fact]
        public void UserName_PropertyIsSettable()
        {
            // Act
            _viewModel.UserName = "Nuevo Usuario";

            // Assert
            Assert.Equal("Nuevo Usuario", _viewModel.UserName);
        }

        [Fact]
        public void DailyAdvice_IsSetOnInitialization()
        {
            DateTime testDate = new DateTime(2024, 5, 7);
            Assert.Equal(DayOfWeek.Tuesday, testDate.DayOfWeek); // Esto debe ser verdadero, martes es DayOfWeek.Tuesday que es 2
    
            var testViewModel = new HomePageViewModel(testDate);

            // Esperando el consejo para el martes
            string expectedAdvice = "Tómate un tiempo para meditar y despejar tu mente.";

            // Verificar que el consejo diario esperado es el correcto
            Assert.Equal(expectedAdvice, testViewModel.DailyAdvice);
        }


        [Fact]
        public void Commands_InitializedCorrectly()
        {
            // Assert
            Assert.NotNull(_viewModel.GoToChatbotCommand);
            Assert.NotNull(_viewModel.RegisterActivityCommand);
            Assert.NotNull(_viewModel.GoToHealthRemindersPageCommand);
            Assert.NotNull(_viewModel.GoToProgressPageCommand);

            // Commands should be executable
            Assert.True(_viewModel.GoToChatbotCommand.CanExecute(null));
            Assert.True(_viewModel.RegisterActivityCommand.CanExecute(null));
            Assert.True(_viewModel.GoToHealthRemindersPageCommand.CanExecute(null));
            Assert.True(_viewModel.GoToProgressPageCommand.CanExecute(null));
        }

    }
}