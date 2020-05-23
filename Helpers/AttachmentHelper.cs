using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AntTrak.Helpers
{
    public class AttachmentHelper
    {

        public static string ShowIcon(string filePath)
        {
            string iconPath;
            switch (Path.GetExtension(filePath))
            {
                case ".bmp":
                    iconPath = "/Icons/bmp.png";
                    break;
                case ".doc":
                    iconPath = "/Icons/doc.png";
                    break;
                case ".docx":
                    iconPath = "/Icons/docx.png";
                    break;
                case ".dotx":
                    iconPath = "/Icons/dotx.png";
                    break;
                case ".jpg":
                    iconPath = "/Icons/jpg.png";
                    break;
                case ".ods":
                    iconPath = "/Icons/ods.png";
                    break;
                case ".odt":
                    iconPath = "/Icons/odt.png";
                    break;
                case ".pdf":
                    iconPath = "/Icons/pdf.png";
                    break;
                case ".png":
                    iconPath = "/Icons/png.png";
                    break;
                case ".rtf":
                    iconPath = "/Icons/rtf.png";
                    break;
                case ".tiff":
                    iconPath = "/Icons/tiff.png";
                    break;
                case ".txt":
                    iconPath = "/Icons/txt.png";
                    break;
                case ".xls":
                    iconPath = "/Icons/xls.png";
                    break;
                case ".xlsx":
                    iconPath = "/Icons/xlsx.png";
                    break;
                default:
                    iconPath = "Icon/unknown.png";
                    break;
            }

            return iconPath;
        }



    }





}