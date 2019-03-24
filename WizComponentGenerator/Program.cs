using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace WizComponentGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectOutputFolder = args[0];
            var outputFolder = args[1];
            var wxsFile = args[2];
            var rootDirectoryId = args[3];
            var applicationFeatureId = args[4];

            var addedDirectories = new Dictionary<string, string>();

            var doc = new XmlDocument();
            doc.Load(wxsFile);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("wi", "http://schemas.microsoft.com/wix/2006/wi");
            nsmgr.AddNamespace("netfx", "http://schemas.microsoft.com/wix/NetFxExtension");

            var wixNode = doc.SelectSingleNode("wi:Wix", nsmgr);
            var productNode = wixNode.SelectSingleNode("wi:Product", nsmgr);

            var directoryNode = productNode;

            do
            {
                directoryNode = directoryNode.SelectSingleNode("wi:Directory", nsmgr);
            }
            while (!directoryNode.Attributes["Id"].Value.Equals(rootDirectoryId));

            var mainDirectoryRefNode = productNode.SelectSingleNode($"wi:DirectoryRef[@Id='{rootDirectoryId}']", nsmgr);
            var featureNode = productNode.SelectSingleNode($"wi:Feature[@Id='{applicationFeatureId}']", nsmgr);
            var rootDirectory = new DirectoryInfo(projectOutputFolder).Name;

            foreach (var filePath in Directory.GetFiles(projectOutputFolder, "*.*", SearchOption.AllDirectories))
            {
                var fileName = Path.GetFileName(filePath);
                var directory = new DirectoryInfo(filePath).Parent.Name;

                //Create the folder in the directory structure of the installer
                if (!directory.Equals(rootDirectory))
                {
                    var directoryId = $"directory_{directory}";

                    if (!addedDirectories.ContainsKey(directory))
                    {
                        addedDirectories.Add(directory, directoryId);

                        var newDirectoryNode = doc.CreateElement("Directory", doc.DocumentElement.NamespaceURI);
                        newDirectoryNode.RemoveAttribute("xmlns");
                        newDirectoryNode.SetAttribute("Id", directoryId);
                        newDirectoryNode.SetAttribute("Name", directory);

                        directoryNode.AppendChild(newDirectoryNode);

                        var newDirectoryRefNode = doc.CreateElement("DirectoryRef", doc.DocumentElement.NamespaceURI);
                        newDirectoryRefNode.SetAttribute("Id", directoryId);
                        newDirectoryRefNode.RemoveAttribute("xmlns");
                        mainDirectoryRefNode.ParentNode.InsertAfter(newDirectoryRefNode, mainDirectoryRefNode);
                    }

                    var directoryRefNode = productNode.SelectSingleNode($"wi:DirectoryRef[@Id='{directoryId}']", nsmgr);

                    AddComponent(doc, directoryRefNode, featureNode, fileName, directory);
                }
                else
                {
                    AddComponent(doc, mainDirectoryRefNode, featureNode, fileName, null);
                }
            }

            doc.Save(outputFolder + "\\Product.wxs");
        }

        private static void AddComponent(XmlDocument doc, XmlNode directoryRefNode, XmlNode featureNode, string fileName, string extraDirectory)
        {
            if (!fileName.EndsWith("pdb"))
            {
                var componentNode = doc.CreateElement("Component", doc.DocumentElement.NamespaceURI);
                componentNode.SetAttribute("Id", fileName);
                componentNode.SetAttribute("Guid", Guid.NewGuid().ToString());
                

                var fileNode = doc.CreateElement("File", doc.DocumentElement.NamespaceURI);
                fileNode.SetAttribute("Id", fileName);

                if (!string.IsNullOrEmpty(extraDirectory))
                {
                    fileNode.SetAttribute("Source", $"$(var.sourceFolder)\\{extraDirectory}\\{fileName}");
                }
                else
                {
                    fileNode.SetAttribute("Source", $"$(var.sourceFolder)\\{fileName}");
                }

                fileNode.SetAttribute("KeyPath", "yes");

                if (fileName.EndsWith("exe"))
                {
                    fileNode.SetAttribute("Checksum", "yes");
                }

                componentNode.AppendChild(fileNode);
                directoryRefNode.AppendChild(componentNode);

                var componentRefNode = doc.CreateElement("ComponentRef", doc.DocumentElement.NamespaceURI);
                componentRefNode.SetAttribute("Id", fileName);
                featureNode.AppendChild(componentRefNode);
            }
        }
    }
}
