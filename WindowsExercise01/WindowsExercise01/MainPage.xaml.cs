using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System.Net.Http.Headers;
using HtmlAgilityPack;




namespace AsyncAwaitDojo
{
public sealed partial class MainPage : Page
{
	public MainPage()
	{
		InitializeComponent();
        
	}

    private void waitingState(bool waiting)
    {
        var pBarVisible = (waiting) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        pBar.Visibility = pBarVisible;
        pageName.Opacity = (waiting) ? 0.5:1.0;
        UrlTextBox.IsEnabled = !waiting;
    }

	private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
	{
		string site;
        string htmlContent;
        string bodyContent_string = "";
        BodyTextBlock.Text = "";
		site = UrlTextBox.Text;

        var webGet = new HtmlWeb();
        waitingState(true);
        HtmlDocument document;
        try
        {
            document = await webGet.LoadFromWebAsync(site);
        }
        catch(System.Net.Http.HttpRequestException exep)
        {
            
            BodyTextBlock.Text = exep.ToString();
            waitingState(false);
            return;
        }
        
        var nodes = document.DocumentNode.Descendants();
        HtmlNode body = null;
        foreach (var n in nodes)
        {
            if (n.Name.Equals("body"))
            {
                bodyContent_string = n.InnerText;
                body = n;
                break;
            }
        }
        if (body == null)
            bodyContent_string = "Error";
                  
        waitingState(false);

        BodyTextBlock.Text = bodyContent_string;
				
	}
}
}