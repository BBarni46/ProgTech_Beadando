using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Beadando1;
using System.Threading;
using System.Windows.Controls;

namespace Beadando1.Tests
{
    [TestClass]
    public class RegisterWindowTests
    {
        private static Thread uiThread;
        private static RegisterWindow window;
        private static ManualResetEvent initCompleted = new ManualResetEvent(false);

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            uiThread = new Thread(() =>
            {
                window = new RegisterWindow(new MainWindow());
                window.Show();
                initCompleted.Set();
                System.Windows.Threading.Dispatcher.Run();
            });

            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();

            initCompleted.WaitOne();
        }

        [TestMethod]
        public void Register_WithEmptyFields_ShowsError()
        {
            window.Dispatcher.Invoke(() =>
            {
                window.UsernameTextBox.Text = "";
                window.PasswordBox.Password = "";
                var registerClick = typeof(RegisterWindow).GetMethod("Register_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                registerClick.Invoke(window, new object[] { null, null });

                Assert.AreEqual("Kérlek töltsd ki az összes mezőt.", window.StatusTextBlock.Text);
            });
        }
    }
}
