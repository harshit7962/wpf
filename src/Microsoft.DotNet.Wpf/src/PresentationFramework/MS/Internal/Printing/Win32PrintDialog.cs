// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !DONOTREFPRINTINGASMMETA


using System;
using System.Printing.Interop;
using System.Printing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Controls;

namespace MS.Internal.Printing
{
    /// <summary>
    /// This entire class is implemented in this file.  However, the class
    /// is marked partial because this class utilizes/implements a marshaler
    /// class that is private to it in another file.  The object is called
    /// PrintDlgExMarshaler and warranted its own file.
    /// </summary>
    internal partial class Win32PrintDialog
    {
        #region Constructor

        /// <summary>
        /// Constructs an instance of the Win32PrintDialog.  This class is used for
        /// displaying the Win32 PrintDlgEx dialog and obtaining a user selected
        /// printer and PrintTicket for a print operation.
        /// </summary>
        public
        Win32PrintDialog()
        {
            _printTicket = null;
            _printQueue = null;
            _minPage = 1;
            _maxPage = 9999;
            _pageRangeSelection = PageRangeSelection.AllPages;
        }

        #endregion Constructor

        #region Internal methods

        /// <summary>
        /// Displays a modal Win32 print dialog to allow the user to select the desired
        /// printer and set the printing options.  The data generated by this method
        /// can be accessed via the properties on the instance of this class.
        /// </summary>
        internal
        UInt32
        ShowDialog()
        {
            UInt32 result = NativeMethods.PD_RESULT_CANCEL;

            //
            // Get the process main window handle
            //
            IntPtr owner = IntPtr.Zero;

            if ((System.Windows.Application.Current != null) &&
                (System.Windows.Application.Current.MainWindow != null))
            {
                System.Windows.Interop.WindowInteropHelper helper =
                    new System.Windows.Interop.WindowInteropHelper(System.Windows.Application.Current.MainWindow);
                owner = helper.Handle;
            }

            try
            {
                if (this._printQueue == null || this._printTicket == null)
                {
                    // Normally printDlgEx.SyncToStruct() probes the printer if both the print queue and print
                    // ticket are not null.
                    // If either is null we probe the printer ourselves
                    // If we dont end users will get notified that printing is disabled *after*
                    // the print dialog has been displayed.

                    ProbeForPrintingSupport();
                }

                //
                // Create a PrintDlgEx instance to invoke the Win32 Print Dialog
                //
                using (PrintDlgExMarshaler printDlgEx = new PrintDlgExMarshaler(owner, this))
                {
                    printDlgEx.SyncToStruct();

                    //
                    // Display the Win32 print dialog
                    //
                    Int32 hr = UnsafeNativeMethods.PrintDlgEx(printDlgEx.UnmanagedPrintDlgEx);
                    if (hr == MS.Win32.NativeMethods.S_OK)
                    {
                        result = printDlgEx.SyncFromStruct();
                    }
                }
            }
            //
            // NOTE:
            // This code was previously catch(PrintingNotSupportedException), but that created a circular dependency
            // between ReachFramework.dll and PresentationFramework.dll. Instead, we now catch Exception, check its full type name
            // and rethrow if it doesn't match. Not perfect, but better than having a circular dependency.
            //
            catch(Exception e)
            {
                if (String.Equals(e.GetType().FullName, "System.Printing.PrintingNotSupportedException", StringComparison.Ordinal))
                {
                    string message = System.Windows.SR.PrintDialogInstallPrintSupportMessageBox;
                    string caption = System.Windows.SR.PrintDialogInstallPrintSupportCaption;

                    bool isRtlCaption = caption != null && caption.Length > 0 && caption[0] == RightToLeftMark;
                    System.Windows.MessageBoxOptions mbOptions = isRtlCaption ? System.Windows.MessageBoxOptions.RtlReading : System.Windows.MessageBoxOptions.None;

                    int type =
                          (int) System.Windows.MessageBoxButton.OK
                        | (int) System.Windows.MessageBoxImage.Information
                        | (int) mbOptions;

                    if (owner == IntPtr.Zero)
                    {
                        owner = MS.Win32.UnsafeNativeMethods.GetActiveWindow();
                    }

                    if(0 != MS.Win32.UnsafeNativeMethods.MessageBox(new HandleRef(null, owner), message, caption, type))
                    {
                         result = NativeMethods.PD_RESULT_CANCEL;
                    }
                }
                else
                {
                    // Not a PrintingNotSupportedException, rethrow
                    throw;
                }
            }

            return result;
        }

        #endregion Internal methods

        #region Internal properties

        internal PrintTicket PrintTicket
        {
            get
            {
                return _printTicket;
            }
            set
            {
                _printTicket = value;
            }
        }

        internal PrintQueue PrintQueue
        {
            get
            {
                return _printQueue;
            }
            set
            {
                _printQueue = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum page number allowed in the page ranges.
        /// </summary>
        internal UInt32 MinPage
        {
            get
            {
                return _minPage;
            }
            set
            {
                _minPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum page number allowed in the page ranges.
        /// </summary>
        internal UInt32 MaxPage
        {
            get
            {
                return _maxPage;
            }
            set
            {
                _maxPage = value;
            }
        }

        /// <summary>
        /// Gets or Sets the PageRangeSelection option for the print dialog.
        /// </summary>
        internal PageRangeSelection PageRangeSelection
        {
            get
            {
                return _pageRangeSelection;
            }
            set
            {
                _pageRangeSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets a PageRange objects used when the PageRangeSelection
        /// option is set to UserPages.
        /// </summary>
        internal PageRange PageRange
        {
            get
            {
                return _pageRange;
            }
            set
            {
                _pageRange = value;
            }
        }

        /// <summary>
        /// Gets or sets a flag to enable/disable the page range control on the dialog.
        /// </summary>
        internal bool PageRangeEnabled
        {
            get
            {
                return _pageRangeEnabled;
            }
            set
            {
                _pageRangeEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a flag to enable/disable the current page selection control on the dialog.
        /// </summary>
        internal bool SelectedPagesEnabled
        {
            get
            {
                return _selectedPagesEnabled;
            }
            set
            {
                _selectedPagesEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a flag to enable/disable the current page control on the dialog.
        /// </summary>
        internal bool CurrentPageEnabled
        {
            get
            {
                return _currentPageEnabled;
            }
            set
            {
                _currentPageEnabled = value;
            }
        }

        #endregion Internal properties

        #region Private methods

        /// <summary>
        /// Probe to see if printing support is installed
        /// </summary>
        private void ProbeForPrintingSupport()
        {
            // Without a print queue object we have to make up a name for the printer.
            // We will just swallow the print queue exception it generates later.
            // We could avoid the exception if we had access to
            // MS.Internal.Printing.Configuration.NativeMethods.BindPTProviderThunk

            string printerName = (this._printQueue != null) ? this._printQueue.FullName : string.Empty;

            try
            {
                // If printer support is not installed this should throw a PrintingNotSupportedException
                using (IDisposable converter = new PrintTicketConverter(printerName, 1))
                {
                }
            }
            catch (PrintQueueException)
            {
                // We can swallow print queue exceptions because they imply that printing
                // support is installed
            }
        }

        #endregion

        #region Private data

        private
        PrintTicket _printTicket;

        private
        PrintQueue _printQueue;

        private
        PageRangeSelection  _pageRangeSelection;

        private
        PageRange           _pageRange;

        private
        bool                _pageRangeEnabled;

        private
        bool                _selectedPagesEnabled;

        private
        bool                _currentPageEnabled;
        
        private
        UInt32              _minPage;

        private
        UInt32              _maxPage;

        private
        const char RightToLeftMark = '\u200F';

        #endregion Private data
    }
}
#endif
