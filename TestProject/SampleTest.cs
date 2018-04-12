using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject
{
    [TestFixture]
    public class SampleTest
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver();
            _driver.Navigate().GoToUrl("https://www.n11.com");
        }

        //1) n11 anasayfasının kontolü.
        public void homePage()
        {
            //Burada sayfanın url'ini kontrol ederek n11 anasayfası olduğu test edilmiştir.
            Assert.AreEqual("https://www.n11.com/", _driver.Url);
        }

        //2) Login ekranın açılması ve bir kullanıcı ile login olunmasının kontrolü.
        public void login()
        {
            //Burada n11 sitesine giriş işlemi test edilmiştir.
            _driver.FindElement(By.ClassName("btnSignIn")).Click();
            _driver.FindElement(By.Id("email")).SendKeys("n11testhesabi@hotmail.com");
            _driver.FindElement(By.Id("password")).SendKeys("n11testmail" + Keys.Enter);
            Thread.Sleep(5000);
        }

        //3)Search alanına 'samsung' yazılıp arama yapılabilmesinin kontrolü.
        public void Search()
        {
            //Burada search alanına samsung yazılıp arama yapılmıştır
            _driver.FindElement(By.Id("searchData")).SendKeys("samsung" + Keys.Enter);
            Thread.Sleep(10000);
        }

        //4)Gelen sayfada samsung ile ilgili sonuçların görüntülenmesinin kontrolü.
        public void SamsungResult()
        {
            //Burada samsung sonuçlarının gösteriliği tablonun id'sinin sayısı test edilmiştir
            int view_list_count = _driver.FindElements(By.XPath("//*[@id='view']")).Count;
            Assert.AreEqual(view_list_count, 1);
            Thread.Sleep(5000);
        }

        //5)Arama sonuçlarından 2. sayfaya tıklanması ve açılan sayfada 2. sayfanın görüntülendiğinin kontrolü.
        public void SecondPage()
        {
            //Burada arama sonuçlarından 2. sayfaya gidilmiştir ve 2. sayfaya gidildiği test edilmiştir.
            _driver.FindElement(By.LinkText("2")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual("https://www.n11.com/arama?q=samsung&pg=2", _driver.Url);
            Console.WriteLine("2. sayfaya geçti");
        }

        //6)Üstten 3. ürünün içindeki 'favorilere ekle' butonuna tıklanmasının kontrolü.
        public void FavoriteProduct()
        {
            //Burada istenilen ürünün favorilere eklenmiştir, favorilere eklenebilmek için sayfa scrool edilmiştir.
            //Ayrıca favorilere eklenen ürün aşağıdaki testte kullanılmak üzere bir degişkene eşitlenmiştir.
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
            js.ExecuteScript("window.scrollBy(0,700);");
            Thread.Sleep(2000);
            IWebElement fav_element = _driver.FindElement(By.XPath("//*[contains(@data-position, '31')]/*[contains(@class, 'proDetail')]/*[contains(@class, 'textImg followBtn')]"));
            //string product_title = _driver.FindElement(By.XPath("//*[contains(@data-position, '31')]/*[contains(@class, 'pro')]/a[@title]")).Text;
            fav_element.Click();
            //Console.WriteLine(product_title);
            Console.WriteLine("element var.");
            Thread.Sleep(2000);
        }

        //7)Ekranın üstündeki 'favorilerim' linkine tıklanması.
        public void FavoritePage()
        {
            //Burada favorilerim sayfasına gidebilmek için sayfa scrool edilmiştir.
            //Ayrıca gerekli mouse hover işlemi yapılmış ve favorilerim linkine tıklanmıştır.
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
            js.ExecuteScript("window.scrollBy(0,-1500);");
            Thread.Sleep(2000);
            Console.WriteLine("scroll gerçekleşti");
            Actions action = new Actions(_driver);
            IWebElement element = _driver.FindElement(By.ClassName("myAccount"));
            Thread.Sleep(5000);
            action.MoveToElement(element).Build().Perform();
            Console.WriteLine("mouse hover gerçekleşti");
            Thread.Sleep(2000);
            _driver.FindElement(By.LinkText("İstek Listem / Favorilerim")).Click();
            Assert.AreEqual("https://www.n11.com/hesabim/istek-listelerim", _driver.Url);
            _driver.FindElement(By.XPath("//a[contains(@href,'https://www.n11.com/hesabim/favorilerim')]")).Click();
            Thread.Sleep(5000);
        }

        //8)Acilan sayfada bir onceki sayfada izlemeye alinmis urunun bulundugununun kontrolü.
        public void FavoriteProductControl(string product_title)
        {
            //Burada yukarıda favoriye eklediğim ürünün sayfada görüntülendiği test edilmiştir.
            string favori_title = _driver.FindElement(By.XPath("//*[contains(@class, 'pro')]/a[@title]")).Text;
            Assert.AreEqual(product_title, favori_title);
            Console.WriteLine("Favori seçilen ürün favorilerde görüntülendi.");
            Thread.Sleep(5000);
        }

        //9)Favorilere alinan bu urunun yanindaki 'Kaldir' butonuna basarak, favorilerimden cıkarılmasının kotrolü.
        public void DeleteFavoriteProduct()
        {
            //Burada favorilerde bulunan ürün silinerek favorilerden çıkarılması test edilmiştir.
            IWebElement element_sil = _driver.FindElement(By.XPath("//*[contains(@class, 'column wishListColumn ')]/*[contains(@class, 'columnContent ')]/*[contains(@class, 'wishProBtns')]/*[contains(@class, 'deleteProFromFavorites')]"));
            element_sil.Click();
            Thread.Sleep(5000);
            IWebElement element_sil_tamam = _driver.FindElement(By.XPath("//*[contains(@class, 'btn btnBlack confirm')]"));
            element_sil_tamam.Click();
            Console.WriteLine("Favori seçilen ürün favorilerden silindi.");
            Thread.Sleep(5000);
        }

        //10)Sayfada bu urunun artik favorilere alinmadiginin onaylanmasi.
        public void DeleteFavoriteProductControl(string product_title)
        {
            //Burada favorileren çıkarılan ürünün sayfada bulunmadığı test edilmiştir.
            IWebElement body = _driver.FindElement(By.TagName("body"));
            Assert.IsFalse(body.Text.Contains(product_title));
            Console.WriteLine("Test başarıyla tamamlanmıştır.");
        }

        [Test]
        public void testApp()
        {
            homePage();
            login();
            Search();
            SamsungResult();
            SecondPage();
            FavoriteProduct();
            //product_title değişkeni aşağıdaki methodlarda kullanılmak için tanımlanmıştır.
            string product_title = _driver.FindElement(By.XPath("//*[contains(@data-position, '31')]/*[contains(@class, 'pro')]/a[@title]")).Text;
            FavoritePage();
            FavoriteProductControl(product_title);
            DeleteFavoriteProduct();
            DeleteFavoriteProductControl(product_title);
        }

    }
}
