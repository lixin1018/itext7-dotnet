/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using iText.IO.Image;
using iText.Kernel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Utils;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Layout {
    public class LayoutTaggingPdf2Test : ExtendedITextTest {
        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/LayoutTaggingPdf2Test/";

        public const String imageName = "Desert.jpg";

        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/LayoutTaggingPdf2Test/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void SimpleDocDefault() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "simpleDocDefault.pdf", new WriterProperties
                ().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            Paragraph h9 = new Paragraph("Header level 9");
            h9.GetAccessibilityProperties().SetRole("H9");
            Paragraph h11 = new Paragraph("Hello World from iText7");
            h11.GetAccessibilityProperties().SetRole("H11");
            document.Add(h9);
            AddSimpleContentToDoc(document, h11);
            document.Close();
            CompareResult("simpleDocDefault");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void SimpleDocNullNsByDefault() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "simpleDocNullNsByDefault.pdf"
                , new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            pdfDocument.GetTagStructureContext().SetDocumentDefaultNamespace(null);
            Document document = new Document(pdfDocument);
            Paragraph h1 = new Paragraph("Header level 1");
            h1.GetAccessibilityProperties().SetRole(StandardRoles.H1);
            Paragraph helloWorldPara = new Paragraph("Hello World from iText7");
            document.Add(h1);
            AddSimpleContentToDoc(document, helloWorldPara);
            document.Close();
            CompareResult("simpleDocNullNsByDefault");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void SimpleDocExplicitlyOldStdNs() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "simpleDocExplicitlyOldStdNs.pdf"
                , new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
            PdfNamespace @namespace = tagsContext.FetchNamespace(StandardNamespaces.PDF_1_7);
            tagsContext.SetDocumentDefaultNamespace(@namespace);
            Document document = new Document(pdfDocument);
            Paragraph h1 = new Paragraph("Header level 1");
            h1.GetAccessibilityProperties().SetRole(StandardRoles.H1);
            Paragraph helloWorldPara = new Paragraph("Hello World from iText7");
            document.Add(h1);
            AddSimpleContentToDoc(document, helloWorldPara);
            document.Close();
            CompareResult("simpleDocExplicitlyOldStdNs");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void CustomRolesMappingPdf2() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "customRolesMappingPdf2.pdf", 
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
            PdfNamespace stdNamespace2 = tagsContext.FetchNamespace(StandardNamespaces.PDF_2_0);
            PdfNamespace xhtmlNs = new PdfNamespace("http://www.w3.org/1999/xhtml");
            PdfNamespace html4Ns = new PdfNamespace("http://www.w3.org/TR/html4");
            String h9 = "H9";
            String h11 = "H11";
            // deliberately mapping to H9 tag
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.h1, h9, stdNamespace2);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, StandardRoles.P, stdNamespace2);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.img, StandardRoles.FIGURE, stdNamespace2);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.ul, StandardRoles.L, stdNamespace2);
            xhtmlNs.AddNamespaceRoleMapping(StandardRoles.SPAN, LayoutTaggingPdf2Test.HtmlRoles.span, xhtmlNs);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.span, StandardRoles.SPAN, stdNamespace2);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.center, StandardRoles.P, stdNamespace2);
            html4Ns.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.center, LayoutTaggingPdf2Test.HtmlRoles.center
                , xhtmlNs);
            // test some tricky mapping cases
            stdNamespace2.AddNamespaceRoleMapping(h9, h11, stdNamespace2);
            stdNamespace2.AddNamespaceRoleMapping(h11, h11, stdNamespace2);
            tagsContext.GetAutoTaggingPointer().SetNamespaceForNewTags(xhtmlNs);
            Document document = new Document(pdfDocument);
            AddContentToDocInCustomNs(pdfDocument, stdNamespace2, xhtmlNs, html4Ns, h11, document);
            document.Close();
            CompareResult("customRolesMappingPdf2");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void CustomRolesMappingPdf17() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "customRolesMappingPdf17.pdf", 
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            PdfNamespace xhtmlNs = new PdfNamespace("http://www.w3.org/1999/xhtml");
            PdfNamespace html4Ns = new PdfNamespace("http://www.w3.org/TR/html4");
            String h9 = "H9";
            String h1 = StandardRoles.H1;
            // deliberately mapping to H9 tag
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.h1, h9);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, StandardRoles.P);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.img, StandardRoles.FIGURE);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.ul, StandardRoles.L);
            xhtmlNs.AddNamespaceRoleMapping(StandardRoles.SPAN, LayoutTaggingPdf2Test.HtmlRoles.span, xhtmlNs);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.span, StandardRoles.SPAN);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.center, "Center");
            html4Ns.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.center, LayoutTaggingPdf2Test.HtmlRoles.center
                , xhtmlNs);
            // test some tricky mapping cases
            pdfDocument.GetStructTreeRoot().AddRoleMapping(h9, h1);
            pdfDocument.GetStructTreeRoot().AddRoleMapping(h1, h1);
            pdfDocument.GetStructTreeRoot().AddRoleMapping("Center", StandardRoles.P);
            pdfDocument.GetStructTreeRoot().AddRoleMapping("I", StandardRoles.SPAN);
            pdfDocument.GetTagStructureContext().SetDocumentDefaultNamespace(null);
            pdfDocument.GetTagStructureContext().GetAutoTaggingPointer().SetNamespaceForNewTags(xhtmlNs);
            Document document = new Document(pdfDocument);
            AddContentToDocInCustomNs(pdfDocument, null, xhtmlNs, html4Ns, h1, document);
            document.Close();
            CompareResult("customRolesMappingPdf17");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithExplicitAndImplicitDefaultNsAtTheSameTime() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithExplicitAndImplicitDefaultNsAtTheSameTime.pdf"
                , new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
            tagsContext.SetDocumentDefaultNamespace(null);
            PdfNamespace explicitDefaultNs = tagsContext.FetchNamespace(StandardNamespaces.PDF_1_7);
            Document document = new Document(pdfDocument);
            Paragraph hPara = new Paragraph("This is header.");
            hPara.GetAccessibilityProperties().SetRole(StandardRoles.H);
            hPara.GetAccessibilityProperties().SetNamespace(explicitDefaultNs);
            document.Add(hPara);
            PdfNamespace xhtmlNs = new PdfNamespace("http://www.w3.org/1999/xhtml");
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.img, StandardRoles.FIGURE, explicitDefaultNs
                );
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.ul, StandardRoles.L);
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(sourceFolder + imageName)).SetWidth(100
                );
            img.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.img);
            img.GetAccessibilityProperties().SetNamespace(xhtmlNs);
            document.Add(img);
            List list = new List().SetListSymbol("-> ");
            list.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.ul);
            list.GetAccessibilityProperties().SetNamespace(xhtmlNs);
            list.Add("list item").Add("list item").Add("list item").Add("list item").Add("list item");
            document.Add(list);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.center, "Center", explicitDefaultNs);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, "Note", explicitDefaultNs);
            explicitDefaultNs.AddNamespaceRoleMapping("Center", StandardRoles.P, explicitDefaultNs);
            explicitDefaultNs.AddNamespaceRoleMapping("Note", "Note");
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.span, "Note");
            pdfDocument.GetStructTreeRoot().AddRoleMapping("Note", StandardRoles.P);
            Paragraph centerPara = new Paragraph("centered text");
            centerPara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.center);
            centerPara.GetAccessibilityProperties().SetNamespace(xhtmlNs);
            Text simpleSpan = new Text("simple p with simple span");
            simpleSpan.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.span);
            simpleSpan.GetAccessibilityProperties().SetNamespace(xhtmlNs);
            Paragraph simplePara = new Paragraph(simpleSpan);
            simplePara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
            simplePara.GetAccessibilityProperties().SetNamespace(xhtmlNs);
            document.Add(centerPara).Add(simplePara);
            pdfDocument.GetStructTreeRoot().AddRoleMapping("I", StandardRoles.SPAN);
            Text iSpan = new Text("cursive span");
            iSpan.GetAccessibilityProperties().SetRole("I");
            document.Add(new Paragraph(iSpan));
            document.Close();
            CompareResult("docWithExplicitAndImplicitDefaultNsAtTheSameTime");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping01() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping01.pdf", 
                    new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
                pdfDocument.SetTagged();
                TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
                tagsContext.SetDocumentDefaultNamespace(null);
                PdfNamespace explicitDefaultNs = tagsContext.FetchNamespace(StandardNamespaces.PDF_1_7);
                Document document = new Document(pdfDocument);
                pdfDocument.GetStructTreeRoot().AddRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, StandardRoles.P);
                Paragraph customRolePara = new Paragraph("Hello world text.");
                customRolePara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
                customRolePara.GetAccessibilityProperties().SetNamespace(explicitDefaultNs);
                document.Add(customRolePara);
                document.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(String.Format(PdfException.RoleInNamespaceIsNotMappedToAnyStandardRole, "p", "http://iso.org/pdf/ssn")))
;
        }

        // compareResult("docWithInvalidMapping01");
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping02() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping02.pdf", 
                    new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
                pdfDocument.SetTagged();
                TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
                tagsContext.SetDocumentDefaultNamespace(null);
                PdfNamespace explicitDefaultNs = tagsContext.FetchNamespace(StandardNamespaces.PDF_1_7);
                Document document = new Document(pdfDocument);
                explicitDefaultNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, StandardRoles.P);
                Paragraph customRolePara = new Paragraph("Hello world text.");
                customRolePara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
                document.Add(customRolePara);
                document.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(String.Format(PdfException.RoleIsNotMappedToAnyStandardRole, "p")))
;
        }

        // compareResult("docWithInvalidMapping02");
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping03() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping03.pdf", 
                    new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
                pdfDocument.SetTagged();
                Document document = new Document(pdfDocument);
                Paragraph customRolePara = new Paragraph("Hello world text.");
                customRolePara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
                document.Add(customRolePara);
                document.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(String.Format(PdfException.RoleInNamespaceIsNotMappedToAnyStandardRole, "p", "http://iso.org/pdf2/ssn")))
;
        }

        // compareResult("docWithInvalidMapping03");
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping04() {
            // TODO this test passes, however it seems, that mingling two standard namespaces in the same tag structure tree should be illegal
            // May be this should be checked if we would implement conforming PDF/UA docs generations in a way PDF/A docs are generated
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping04.pdf", 
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            TagStructureContext tagsCntxt = pdfDocument.GetTagStructureContext();
            PdfNamespace stdNs2 = tagsCntxt.FetchNamespace(StandardNamespaces.PDF_2_0);
            stdNs2.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, StandardRoles.P);
            Document document = new Document(pdfDocument);
            Paragraph customRolePara = new Paragraph("Hello world text.");
            customRolePara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
            document.Add(customRolePara);
            document.Close();
            CompareResult("docWithInvalidMapping04");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping05() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping05.pdf", 
                    new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
                pdfDocument.SetTagged();
                Document document = new Document(pdfDocument);
                // deliberately creating namespace via constructor instead of using TagStructureContext#fetchNamespace
                PdfNamespace stdNs2 = new PdfNamespace(StandardNamespaces.PDF_2_0);
                stdNs2.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.p, StandardRoles.P, stdNs2);
                Paragraph customRolePara = new Paragraph("Hello world text.");
                customRolePara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
                customRolePara.GetAccessibilityProperties().SetNamespace(stdNs2);
                document.Add(customRolePara);
                Paragraph customRolePara2 = new Paragraph("Hello world text.");
                customRolePara2.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
                // not explicitly setting namespace that we've manually created. This will lead to the situation, when
                // /Namespaces entry in StructTreeRoot would have two different namespace dictionaries with the same name.
                document.Add(customRolePara2);
                document.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(String.Format(PdfException.RoleInNamespaceIsNotMappedToAnyStandardRole, "p", "http://iso.org/pdf2/ssn")))
;
        }

        //         compareResult("docWithInvalidMapping05");
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping06() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping06.pdf", 
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            TagStructureContext tagCntxt = pdfDocument.GetTagStructureContext();
            PdfNamespace pointerNs = tagCntxt.FetchNamespace(StandardNamespaces.PDF_2_0);
            pointerNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.span, StandardRoles.SPAN, pointerNs);
            // deliberately creating namespace via constructor instead of using TagStructureContext#fetchNamespace
            PdfNamespace stdNs2 = new PdfNamespace(StandardNamespaces.PDF_2_0);
            stdNs2.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.span, StandardRoles.EM, stdNs2);
            Text customRolePText1 = new Text("Hello world text 1.");
            customRolePText1.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.span);
            customRolePText1.GetAccessibilityProperties().SetNamespace(stdNs2);
            document.Add(new Paragraph(customRolePText1));
            Text customRolePText2 = new Text("Hello world text 2.");
            customRolePText2.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.span);
            // not explicitly setting namespace that we've manually created. This will lead to the situation, when
            // /Namespaces entry in StructTreeRoot would have two different namespace dictionaries with the same name.
            document.Add(new Paragraph(customRolePText2));
            document.Close();
            CompareResult("docWithInvalidMapping06");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.CANNOT_RESOLVE_ROLE_IN_NAMESPACE_TOO_MUCH_TRANSITIVE_MAPPINGS, Count
             = 1)]
        public virtual void DocWithInvalidMapping07() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping07.pdf", 
                    new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
                pdfDocument.SetTagged();
                Document document = new Document(pdfDocument);
                PdfNamespace stdNs2 = pdfDocument.GetTagStructureContext().FetchNamespace(StandardNamespaces.PDF_2_0);
                int numOfTransitiveMappings = 120;
                String prevRole = LayoutTaggingPdf2Test.HtmlRoles.span;
                for (int i = 0; i < numOfTransitiveMappings; ++i) {
                    String nextRole = "span" + i;
                    stdNs2.AddNamespaceRoleMapping(prevRole, nextRole, stdNs2);
                    prevRole = nextRole;
                }
                stdNs2.AddNamespaceRoleMapping(prevRole, StandardRoles.SPAN, stdNs2);
                Text customRolePText1 = new Text("Hello world text.");
                customRolePText1.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.span);
                customRolePText1.GetAccessibilityProperties().SetNamespace(stdNs2);
                document.Add(new Paragraph(customRolePText1));
                document.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(String.Format(PdfException.RoleInNamespaceIsNotMappedToAnyStandardRole, "span", "http://iso.org/pdf2/ssn")))
;
        }

        //        compareResult("docWithInvalidMapping07");
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void DocWithInvalidMapping08() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping08.pdf", 
                    new WriterProperties().SetPdfVersion(PdfVersion.PDF_1_7)));
                pdfDocument.SetTagged();
                Document document = new Document(pdfDocument);
                Paragraph h9Para = new Paragraph("Header level 9");
                h9Para.GetAccessibilityProperties().SetRole("H9");
                document.Add(h9Para);
                document.Close();
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(String.Format(PdfException.RoleIsNotMappedToAnyStandardRole, "H9")))
;
        }

        //        compareResult("docWithInvalidMapping08");
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.CREATED_ROOT_TAG_HAS_MAPPING)]
        public virtual void DocWithInvalidMapping09() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping09.pdf", 
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
            PdfNamespace ssn2 = tagsContext.FetchNamespace(StandardNamespaces.PDF_2_0);
            ssn2.AddNamespaceRoleMapping(StandardRoles.DOCUMENT, "Book", ssn2);
            Document document = new Document(pdfDocument);
            document.Add(new Paragraph("hello world; root tag mapping"));
            document.Close();
            CompareResult("docWithInvalidMapping09");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.CREATED_ROOT_TAG_HAS_MAPPING)]
        public virtual void DocWithInvalidMapping10() {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(destinationFolder + "docWithInvalidMapping10.pdf", 
                new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            TagStructureContext tagsContext = pdfDocument.GetTagStructureContext();
            PdfNamespace ssn2 = tagsContext.FetchNamespace(StandardNamespaces.PDF_2_0);
            ssn2.AddNamespaceRoleMapping(StandardRoles.DOCUMENT, "Book", ssn2);
            ssn2.AddNamespaceRoleMapping("Book", StandardRoles.PART, ssn2);
            Document document = new Document(pdfDocument);
            document.Add(new Paragraph("hello world; root tag mapping"));
            document.Close();
            CompareResult("docWithInvalidMapping10");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void StampTest01() {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourceFolder + "simpleDocOldStdNs.pdf"), new PdfWriter
                (destinationFolder + "stampTest01.pdf", new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE)).Add(new AreaBreak(AreaBreakType.NEXT_PAGE)).Add(new Paragraph
                ("stamped text"));
            document.Close();
            CompareResult("stampTest01");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void StampTest02() {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourceFolder + "simpleDocNoNs.pdf"), new PdfWriter
                (destinationFolder + "stampTest02.pdf", new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE)).Add(new AreaBreak(AreaBreakType.NEXT_PAGE)).Add(new Paragraph
                ("stamped text"));
            document.Close();
            CompareResult("stampTest02");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void StampTest03() {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourceFolder + "simpleDocNewStdNs.pdf"), new PdfWriter
                (destinationFolder + "stampTest03.pdf", new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE)).Add(new AreaBreak(AreaBreakType.NEXT_PAGE)).Add(new Paragraph
                ("stamped text"));
            document.Close();
            CompareResult("stampTest03");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void StampTest04() {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourceFolder + "simpleDoc1_7.pdf"), new PdfWriter(
                destinationFolder + "stampTest04.pdf", new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE)).Add(new AreaBreak(AreaBreakType.NEXT_PAGE)).Add(new Paragraph
                ("stamped text"));
            document.Close();
            CompareResult("stampTest04");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void StampTest05() {
            PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourceFolder + "simpleDocNewStdNs.pdf"), new PdfWriter
                (destinationFolder + "stampTest05.pdf", new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)));
            TagStructureContext tagCntxt = pdfDocument.GetTagStructureContext();
            PdfNamespace xhtmlNs = tagCntxt.FetchNamespace("http://www.w3.org/1999/xhtml");
            PdfNamespace ssn2 = tagCntxt.FetchNamespace(StandardNamespaces.PDF_2_0);
            xhtmlNs.AddNamespaceRoleMapping(LayoutTaggingPdf2Test.HtmlRoles.ul, StandardRoles.L, ssn2);
            TagTreePointer pointer = new TagTreePointer(pdfDocument);
            pointer.MoveToKid(StandardRoles.TABLE).MoveToKid(StandardRoles.TR).MoveToKid(1, StandardRoles.TD).MoveToKid
                (StandardRoles.L);
            pointer.SetRole(LayoutTaggingPdf2Test.HtmlRoles.ul).GetProperties().SetNamespace(xhtmlNs);
            pdfDocument.Close();
            CompareResult("stampTest05");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void CopyTest01() {
            PdfDocument srcPdf = new PdfDocument(new PdfReader(sourceFolder + "simpleDocNewStdNs.pdf"));
            PdfDocument outPdf = new PdfDocument(new PdfWriter(destinationFolder + "copyTest01.pdf", new WriterProperties
                ().SetPdfVersion(PdfVersion.PDF_2_0)));
            outPdf.SetTagged();
            srcPdf.CopyPagesTo(1, 1, outPdf);
            srcPdf.Close();
            outPdf.Close();
            CompareResult("copyTest01");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        [NUnit.Framework.Test]
        public virtual void CopyTest02() {
            PdfDocument srcPdf = new PdfDocument(new PdfReader(sourceFolder + "docSeveralNs.pdf"));
            PdfDocument outPdf = new PdfDocument(new PdfWriter(destinationFolder + "copyTest02.pdf", new WriterProperties
                ().SetPdfVersion(PdfVersion.PDF_2_0)));
            outPdf.SetTagged();
            srcPdf.CopyPagesTo(1, 1, outPdf);
            srcPdf.Close();
            outPdf.Close();
            CompareResult("copyTest02");
        }

        private class HtmlRoles {
            internal static String h1 = "h1";

            internal static String p = "p";

            internal static String img = "img";

            internal static String ul = "ul";

            internal static String center = "center";

            internal static String span = "span";
        }

        /// <exception cref="System.UriFormatException"/>
        private void AddSimpleContentToDoc(Document document, Paragraph p2) {
            iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(sourceFolder + imageName
                )).SetWidth(100);
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            for (int k = 0; k < 5; k++) {
                table.AddCell(p2);
                List list = new List().SetListSymbol("-> ");
                list.Add("list item").Add("list item").Add("list item").Add("list item").Add("list item");
                Cell cell = new Cell().Add(list);
                table.AddCell(cell);
                Cell c = new Cell().Add(img);
                table.AddCell(c);
                Table innerTable = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
                int j = 0;
                while (j < 9) {
                    innerTable.AddCell("Hi");
                    j++;
                }
                table.AddCell(innerTable);
            }
            document.Add(table);
        }

        /// <exception cref="System.UriFormatException"/>
        private void AddContentToDocInCustomNs(PdfDocument pdfDocument, PdfNamespace defaultNamespace, PdfNamespace
             xhtmlNs, PdfNamespace html4Ns, String hnRole, Document document) {
            Paragraph h1P = new Paragraph("Header level 1");
            h1P.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.h1);
            Paragraph helloWorldPara = new Paragraph("Hello World from iText7");
            helloWorldPara.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.p);
            iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(sourceFolder + imageName
                )).SetWidth(100);
            img.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.img);
            document.Add(h1P);
            document.Add(helloWorldPara);
            document.Add(img);
            pdfDocument.GetTagStructureContext().GetAutoTaggingPointer().SetNamespaceForNewTags(defaultNamespace);
            List list = new List().SetListSymbol("-> ");
            list.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.ul);
            list.GetAccessibilityProperties().SetNamespace(xhtmlNs);
            list.Add("list item").Add("list item").Add("list item").Add("list item").Add(new ListItem("list item"));
            document.Add(list);
            Paragraph center = new Paragraph("centered text").SetTextAlignment(TextAlignment.CENTER);
            center.GetAccessibilityProperties().SetRole(LayoutTaggingPdf2Test.HtmlRoles.center);
            center.GetAccessibilityProperties().SetNamespace(html4Ns);
            document.Add(center);
            Paragraph h11Para = new Paragraph("Heading level 11");
            h11Para.GetAccessibilityProperties().SetRole(hnRole);
            document.Add(h11Para);
            if (defaultNamespace == null) {
                Text i = new Text("italic text");
                i.GetAccessibilityProperties().SetRole("I");
                Paragraph pi = new Paragraph(i.SetItalic());
                document.Add(pi);
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        private void CompareResult(String testName) {
            String outFileName = testName + ".pdf";
            String cmpFileName = "cmp_" + outFileName;
            CompareTool compareTool = new CompareTool();
            String outPdf = destinationFolder + outFileName;
            String cmpPdf = sourceFolder + cmpFileName;
            String contentDifferences = compareTool.CompareByContent(outPdf, cmpPdf, destinationFolder, testName + "Diff_"
                );
            String taggedStructureDifferences = compareTool.CompareTagStructures(outPdf, cmpPdf);
            String errorMessage = "";
            errorMessage += taggedStructureDifferences == null ? "" : taggedStructureDifferences + "\n";
            errorMessage += contentDifferences == null ? "" : contentDifferences;
            if (!String.IsNullOrEmpty(errorMessage)) {
                NUnit.Framework.Assert.Fail(errorMessage);
            }
        }
    }
}
