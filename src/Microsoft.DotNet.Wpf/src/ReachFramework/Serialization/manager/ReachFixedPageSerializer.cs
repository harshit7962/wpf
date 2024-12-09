﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*++

    Abstract:
        This file contains the definition of a class that
        defines the common functionality required to serialize
        a FixedPage

--*/
using System.ComponentModel;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps.Packaging;
using System.Xml;
using MS.Utility;

namespace System.Windows.Xps.Serialization
{
    /// <summary>
    /// Class defining common functionality required to
    /// serialize a FixedPage.
    /// </summary>
    internal class FixedPageSerializer :
                   ReachSerializer
    {
        #region Constructor

        /// <summary>
        /// Constructor for class FixedPageSerializer
        /// </summary>
        /// <param name="manager">
        /// The serialization manager, the services of which are
        /// used later in the serialization process of the type.
        /// </param>
        public
        FixedPageSerializer(
            PackageSerializationManager manager
            ) :
        base(manager)
        {

        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// The main method that is called to serialize a FixedPage.
        /// </summary>
        /// <param name="serializedObject">
        /// Instance of object to be serialized.
        /// </param>
        public
        override
        void
        SerializeObject(
            Object serializedObject
            )
        {
            //
            // Create the ImageTable required by the Type Converters
            // The Image table at this time is shared / currentPage
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageImageTable = new Dictionary<int, Uri>();
            //
            // Create the ColorContextTable required by the Type Converters
            // The Image table at this time is shared / currentPage
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageColorContextTable = new Dictionary<int, Uri>();
            //
            // Create the ResourceDictionaryTable required by the Type Converters
            // The Image table at this time is shared / currentPage
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageResourceDictionaryTable = new Dictionary<int, Uri>();

            base.SerializeObject(serializedObject);
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// The main method that is called to serialize the FixedPage
        /// and that is usually called from within the serialization manager
        /// when a node in the graph of objects is at a turn where it should
        /// be serialized.
        /// </summary>
        /// <param name="serializedProperty">
        /// The context of the property being serialized at this time and
        /// it points internally to the object encapsulated by that node.
        /// </param>
        internal
        override
        void
        SerializeObject(
            SerializablePropertyContext serializedProperty
            )
        {
            //
            // Create the ImageTable required by the Type Converters
            // The Image table at this time is shared / currentPage
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageImageTable = new Dictionary<int, Uri>();
            //
            // Create the ColorContextTable required by the Type Converters
            // The Image table at this time is shared / currentPage
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageColorContextTable = new Dictionary<int, Uri>();
            //
            // Create the ResourceDictionaryTable required by the Type Converters
            // The Image table at this time is shared / currentPage
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageResourceDictionaryTable = new Dictionary<int, Uri>();

            base.SerializeObject(serializedProperty);
        }

        /// <summary>
        /// The method is called once the object data is discovered at that
        /// point of the serialization process.
        /// </summary>
        /// <param name="serializableObjectContext">
        /// The context of the object to be serialized at this time.
        /// </param>
        internal
        override
        void
        PersistObjectData(
            SerializableObjectContext serializableObjectContext
            )
        {
            Toolbox.EmitEvent(EventTrace.Event.WClientDRXSerializationBegin);

            ArgumentNullException.ThrowIfNull(serializableObjectContext);

            if (SerializationManager is IXpsSerializationManager)
            {
                (SerializationManager as IXpsSerializationManager).RegisterPageStart();
            }

            FixedPage fixedPage = serializableObjectContext.TargetObject as FixedPage;

            ReachTreeWalker treeWalker = new ReachTreeWalker(this);
            treeWalker.SerializeLinksInFixedPage((FixedPage)serializableObjectContext.TargetObject);

            String xmlnsForType = SerializationManager.GetXmlNSForType(serializableObjectContext.TargetObject.GetType());

            if (xmlnsForType == null)
            {
                XmlWriter.WriteStartElement(serializableObjectContext.Name);
            }
            else
            {
                XmlWriter.WriteStartElement(serializableObjectContext.Name);

                XmlWriter.WriteAttributeString(XpsS0Markup.Xmlns, xmlnsForType);
                XmlWriter.WriteAttributeString(XpsS0Markup.XmlnsX, XpsS0Markup.XmlnsXSchema);

                XmlLanguage language = fixedPage.Language;
                if (language == null)
                {
                    //If the language property is null, assign the language to the default
                    language = XmlLanguage.GetLanguage(XpsS0Markup.XmlLangValue);
                }

                SerializationManager.Language = language;

                XmlWriter.WriteAttributeString(XpsS0Markup.XmlLang, language.ToString());
            }
            {
                Size fixedPageSize = new Size(fixedPage.Width, fixedPage.Height);
                ((IXpsSerializationManager)SerializationManager).FixedPageSize = fixedPageSize;

                //
                // Before we serialize any properties on the FixedPage, we need to
                // serialize the FixedPage as a Visual
                //
                Visual fixedPageAsVisual = serializableObjectContext.TargetObject as Visual;

                bool needEndVisual = false;

                if (fixedPageAsVisual != null)
                {
                    needEndVisual = SerializePageAsVisual(fixedPageAsVisual);
                }

                if (serializableObjectContext.IsComplexValue)
                {
                    PrintTicket printTicket = ((IXpsSerializationManager)SerializationManager).FixedPagePrintTicket;

                    if (printTicket != null)
                    {
                        PrintTicketSerializer serializer = new PrintTicketSerializer(SerializationManager);
                        serializer.SerializeObject(printTicket);
                        ((IXpsSerializationManager)SerializationManager).FixedPagePrintTicket = null;
                    }

                    SerializeObjectCore(serializableObjectContext);
                }
                else
                {
                    throw new XpsSerializationException(SR.ReachSerialization_WrongPropertyTypeForFixedPage);
                }

                if (needEndVisual)
                {
                    XmlWriter pageWriter = ((XpsSerializationManager)SerializationManager).
                                                      PackagingPolicy.AcquireXmlWriterForPage();

                    XmlWriter resWriter = ((XpsSerializationManager)SerializationManager).
                                                      PackagingPolicy.AcquireXmlWriterForResourceDictionary();


                    VisualTreeFlattener flattener = ((IXpsSerializationManager)SerializationManager).
                                                      VisualSerializationService.AcquireVisualTreeFlattener(resWriter,
                                                                                                            pageWriter,
                                                                                                            fixedPageSize);

                    flattener.EndVisual();
                }

            }

            ((XpsSerializationManager)SerializationManager).PackagingPolicy.PreCommitCurrentPage();

            //
            // Copy hyperlinks into stream
            //
            treeWalker.CommitHyperlinks();

            XmlWriter.WriteEndElement();
            //
            //Release used resources
            //
            XmlWriter = null;
            //
            // Free the image table in use for this page
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageImageTable = null;
            //
            // Free the colorContext table in use for this page
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageColorContextTable = null;
            //
            // Free the resourceDictionary table in use for this page
            //
            ((XpsSerializationManager)SerializationManager).ResourcePolicy.CurrentPageResourceDictionaryTable = null;

            ((IXpsSerializationManager)SerializationManager).VisualSerializationService.ReleaseVisualTreeFlattener();

            if (SerializationManager is IXpsSerializationManager)
            {
                (SerializationManager as IXpsSerializationManager).RegisterPageEnd();
            }

            //
            // Signal to any registered callers that the Page has been serialized
            //
            XpsSerializationProgressChangedEventArgs progressEvent =
            new XpsSerializationProgressChangedEventArgs(XpsWritingProgressChangeLevel.FixedPageWritingProgress,
                                                         0,
                                                         0,
                                                         null);

            ((IXpsSerializationManager)SerializationManager).OnXPSSerializationProgressChanged(progressEvent);

            Toolbox.EmitEvent(EventTrace.Event.WClientDRXSerializationEnd);
        }

        /// <summary>
        /// This method is the one that writes out the attribute within
        /// the xml stream when serializing simple properties.
        /// </summary>
        /// <param name="serializablePropertyContext">
        /// The property that is to be serialized as an attribute at this time.
        /// </param>
        internal
        override
        void
        WriteSerializedAttribute(
            SerializablePropertyContext serializablePropertyContext
            )
        {
            ArgumentNullException.ThrowIfNull(serializablePropertyContext);

            String attributeValue = String.Empty;

            attributeValue = GetValueOfAttributeAsString(serializablePropertyContext);

            if ((attributeValue != null) &&
                 (attributeValue.Length > 0))
            {
                //
                // Emit name="value" attribute
                //
                XmlWriter.WriteAttributeString(serializablePropertyContext.Name, attributeValue);
            }
        }

        /// <summary>
        /// Converts the Value of the Attribute to a String by calling into
        /// the appropriate type converters.
        /// </summary>
        /// <param name="serializablePropertyContext">
        /// The property that is to be serialized as an attribute at this time.
        /// </param>
        private
        String
        GetValueOfAttributeAsString(
            SerializablePropertyContext serializablePropertyContext
            )
        {
            ArgumentNullException.ThrowIfNull(serializablePropertyContext);

            String valueAsString = null;
            Object targetObjectContainingProperty = serializablePropertyContext.TargetObject;
            Object propertyValue = serializablePropertyContext.Value;

            if (propertyValue != null)
            {
                TypeConverter typeConverter = serializablePropertyContext.TypeConverter;

                valueAsString = typeConverter.ConvertToInvariantString(new XpsTokenContext(SerializationManager,
                                                                                             serializablePropertyContext),
                                                                       propertyValue);


                if (propertyValue is Type)
                {
                    int index = valueAsString.LastIndexOf('.');
                    valueAsString = string.Concat(
                        XpsSerializationManager.TypeOfString,
                        index > 0 ? valueAsString.AsSpan(index + 1) : valueAsString,
                        "}");
                }
            }
            else
            {
                valueAsString = XpsSerializationManager.NullString;
            }

            return valueAsString;
        }

        #endregion Internal Methods

        #region Public Properties

        /// <summary>
        /// Queries / Set the XmlWriter for a FixedPage.
        /// </summary>
        public
        override
        XmlWriter
        XmlWriter
        {
            get
            {
                if (base.XmlWriter == null)
                {
                    base.XmlWriter = SerializationManager.AcquireXmlWriter(typeof(FixedPage));
                }

                return base.XmlWriter;
            }

            set
            {
                base.XmlWriter = null;
                SerializationManager.ReleaseXmlWriter(typeof(FixedPage));
            }
        }

        #endregion Public Properties

        #region Private Methods

        private
        bool
        SerializePageAsVisual(
            Visual fixedPageAsVisual
            )
        {
            bool needEndVisual = false;

            ReachVisualSerializer serializer = new ReachVisualSerializer(SerializationManager);

            if (serializer != null)
            {
                needEndVisual = serializer.SerializeDisguisedVisual(fixedPageAsVisual);
            }
            else
            {
                throw new XpsSerializationException(SR.ReachSerialization_NoSerializer);
            }

            return needEndVisual;
        }

        #endregion Private Methods
    };
}
