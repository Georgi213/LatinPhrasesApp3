using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LatinPhrasesApp.Models;
using Xamarin.Forms;
using LatinPhrasesApp.ViewModels;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Media;
using Xamarin.Essentials;
using Plugin.Permissions;

namespace LatinPhrasesApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAuthorPage : ContentPage
	{
        private readonly TaskCompletionSource<LatinPhrase> _taskCompletionSource;
        private MyAuthorsViewModel _viewModel;
        private readonly Action<LatinPhrase> _addAuthorAction;


        public Task<LatinPhrase> GetNewPhraseAsync()
        {
            return _taskCompletionSource.Task;
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Get the user input from the Entry controls
            var newAuthor = GetEnteredAuthor();

            // Call the AddPhrase action
            _addAuthorAction(newAuthor);

            // Navigate back to the previous page
            await Navigation.PopModalAsync();
        }
        public AddAuthorPage(Action<LatinPhrase> addAuthorAction)
        {
            InitializeComponent();
            _addAuthorAction = addAuthorAction;
            BindingContext = _viewModel;
        }
        private async void OnSelectImageButtonClicked(object sender, EventArgs e)
        {
            var readStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            var writeStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (readStatus == PermissionStatus.Granted && writeStatus == PermissionStatus.Granted)
            {
                var photo = await CrossMedia.Current.PickPhotoAsync();
                if (photo != null)
                {
                    var stream = photo.GetStream();
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var imagePath = Path.Combine(FileSystem.CacheDirectory, Path.GetRandomFileName());
                    File.WriteAllBytes(imagePath, memoryStream.ToArray());

                    Portrait.Source = ImageSource.FromFile(imagePath);
                }
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to select images.", "OK");
                CrossPermissions.Current.OpenAppSettings();
            }
        }
        public interface IPhotoPickerService
        {
            Task<Stream> PickPhotoAsync();
        }
        private LatinPhrase GetEnteredAuthor()
        {
            string name = Name.Text;
            string latin = Latin.Text;
            string portrait = (Portrait.Source as FileImageSource)?.File;

            return new LatinPhrase
            {
                Name = name,
                Latin = latin,
                Portrait = portrait
            };
        }
    }
}