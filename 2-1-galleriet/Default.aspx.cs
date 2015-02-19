using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using _2_1_galleriet.Model;

namespace _2_1_galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        private ImageGallery _gallery;

        private ImageGallery Gallery
        {
            get { return _gallery ?? (_gallery = new ImageGallery()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Visar rättmeddelande om en bild har laddats upp
            SuccessMessageLiteral.Text = Page.GetTempData("SuccessMessage") as string;
            SuccessMessagePanel.Visible = !String.IsNullOrWhiteSpace(SuccessMessageLiteral.Text);

            //Sätter bildnamnet i url:en på Image-kontrollen till det bildnamn som finns i query-parametern "name" 
            string name = Request.QueryString["name"];
            if (name != null)
            {
                BigImagePanel.Visible = true;
                BigImage.ImageUrl = String.Format("~/Images/{0}", name);
            }
        }

        //Returnerar namnen på de filer som finns i Images/Thumbnails till Repeater-kontrollen
        public IEnumerable<dynamic> ThumbRepeater_GetData()
        {
            return (from name in Gallery.GetImagesNames()
                    select new ThumbPicture
                    {
                        Name = name
                    }).AsEnumerable();
        }

        //Ser till att den valda tumnageln markeras, genom att ändra CSS-klassen på den Hyperlink-kontroll 
        //som har samma namn i sin NavigateUrl som finns i query-parametern "name"
        protected void ThumbRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((HyperLink)e.Item.FindControl("ThumbHyperLink")).NavigateUrl ==
                    String.Format("~/Default.aspx?name={0}", Request.QueryString["name"]))
                {
                    ((HyperLink)e.Item.FindControl("ThumbHyperLink")).CssClass = "chosen";
                }
            }
        }

        //Händelsehanterare kopplad till UploadButton
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            CustomValidator cv = new CustomValidator();

            if (FileUploader.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(FileUploader.PostedFile.FileName);

                    Stream fileContent = FileUploader.FileContent;

                    string savedFileName = Gallery.SaveImage(fileContent, fileName);

                    Page.SetTempData("SuccessMessage", "Bilden har sparats");

                    Response.RedirectToRoute("Index", new { name = savedFileName });
                }
                catch (ArgumentException ex)
                {
                    cv.IsValid = false;
                    cv.ErrorMessage = ex.Message;
                    this.Page.Validators.Add(cv);
                }
                catch (Exception)
                {
                    cv.IsValid = false;
                    cv.ErrorMessage = "Ett fel inträffade då bilden skulle överföras";
                    this.Page.Validators.Add(cv);
                }
            }
            else
            {
                cv.IsValid = false;
                cv.ErrorMessage = "En fil måste väljas.";
                this.Page.Validators.Add(cv);
            }
        }
    }
}