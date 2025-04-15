namespace Consilium.Maui {
    public partial class App : Application {
        public App(IServiceProvider services) {
            InitializeComponent();
            InitializeComponent();
            Services = services;
        }

        public IServiceProvider Services { get; }

        protected override Window CreateWindow(IActivationState? activationState) {
            return new Window(new AppShell());
        }
    }
}