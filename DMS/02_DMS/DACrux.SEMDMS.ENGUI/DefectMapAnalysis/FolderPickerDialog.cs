using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DACrux.SEMDMS.ENGUI
{
    /// <summary>
    /// 탐색기 형식(파일 열기 창과 같은 모양)으로 폴더를 고르는 창. (요청 2026-10-07)
    /// 왜: .NET 4.0 의 FolderBrowserDialog 는 옛 트리 형식이라 깊은 폴더를 찾기 어렵다.
    /// 방법: Windows 의 공용 파일 대화상자(IFileOpenDialog, Vista 이후)를 FOS_PICKFOLDERS(폴더 선택) 옵션으로 연다.
    ///       COM 생성 / 표시가 실패하는 환경이면 기존 FolderBrowserDialog 로 대신 연다.
    /// </summary>
    internal static class FolderPickerDialog
    {
        private const uint FOS_NOCHANGEDIR = 0x00000008;
        private const uint FOS_PICKFOLDERS = 0x00000020;
        private const uint FOS_FORCEFILESYSTEM = 0x00000040;
        private const uint FOS_PATHMUSTEXIST = 0x00000800;
        private const uint SIGDN_FILESYSPATH = 0x80058000;
        private const int ERROR_CANCELLED = unchecked((int)0x800704C7);

        /// <summary>
        /// 폴더를 고른다. 취소하면 null.
        /// 처리: 탐색기 형식 창 생성 -> 옵션(폴더 선택 / 실제 경로만) -> 시작 폴더 -> 표시 -> 선택 항목의 파일 시스템 경로.
        /// </summary>
        public static string Show(IWin32Window owner, string title, string initialFolder)
        {
            IFileDialog dialog = null;
            try
            {
                dialog = (IFileDialog)new FileOpenDialogCom();
            }
            catch (Exception)
            {
                // COM 등록이 없거나(오래된 Windows) 형변환이 안 되면 기존 창으로 연다
                return ShowLegacy(owner, title, initialFolder);
            }

            try
            {
                uint options;
                dialog.GetOptions(out options);
                dialog.SetOptions(options | FOS_PICKFOLDERS | FOS_FORCEFILESYSTEM | FOS_PATHMUSTEXIST | FOS_NOCHANGEDIR);
                if (!string.IsNullOrEmpty(title)) dialog.SetTitle(title);
                if (!string.IsNullOrEmpty(initialFolder) && Directory.Exists(initialFolder))
                {
                    IShellItem folder = CreateItem(initialFolder);
                    if (folder != null) dialog.SetFolder(folder);
                }

                int hr = dialog.Show(owner == null ? IntPtr.Zero : owner.Handle);
                if (hr == ERROR_CANCELLED) return null;
                if (hr != 0) Marshal.ThrowExceptionForHR(hr);

                IShellItem result;
                dialog.GetResult(out result);
                string path;
                result.GetDisplayName(SIGDN_FILESYSPATH, out path);
                return path;
            }
            finally
            {
                Marshal.ReleaseComObject(dialog);
            }
        }

        /// <summary>화면 검사용: 탐색기 형식 창 객체를 만들고 옵션을 넣을 수 있는지만 확인한다(창은 띄우지 않음).</summary>
        internal static bool CanUseModernDialog()
        {
            try
            {
                IFileDialog dialog = (IFileDialog)new FileOpenDialogCom();
                uint options;
                dialog.GetOptions(out options);
                dialog.SetOptions(options | FOS_PICKFOLDERS);
                dialog.GetOptions(out options);
                Marshal.ReleaseComObject(dialog);
                return (options & FOS_PICKFOLDERS) != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>탐색기 형식 창을 쓸 수 없을 때의 기존 폴더 선택 창.</summary>
        private static string ShowLegacy(IWin32Window owner, string title, string initialFolder)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = title;
                if (!string.IsNullOrEmpty(initialFolder) && Directory.Exists(initialFolder)) dialog.SelectedPath = initialFolder;
                return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.SelectedPath : null;
            }
        }

        private static IShellItem CreateItem(string path)
        {
            try
            {
                Guid iid = typeof(IShellItem).GUID;
                IShellItem item;
                SHCreateItemFromParsingName(path, IntPtr.Zero, ref iid, out item);
                return item;
            }
            catch (Exception)
            {
                return null; // 시작 폴더를 못 정해도 창은 연다
            }
        }

        #region COM 선언 (ShObjIdl.h 의 순서 그대로, 순서가 바뀌면 다른 함수가 호출된다)

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, out IShellItem ppv);

        [ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
        private class FileOpenDialogCom
        {
        }

        [ComImport, Guid("42f85136-db7e-439c-85f1-e4075d135fc8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileDialog
        {
            [PreserveSig] int Show(IntPtr parent);                     // IModalWindow
            void SetFileTypes(uint cFileTypes, IntPtr rgFilterSpec);
            void SetFileTypeIndex(uint iFileType);
            void GetFileTypeIndex(out uint piFileType);
            void Advise(IntPtr pfde, out uint pdwCookie);
            void Unadvise(uint dwCookie);
            void SetOptions(uint fos);
            void GetOptions(out uint pfos);
            void SetDefaultFolder(IShellItem psi);
            void SetFolder(IShellItem psi);
            void GetFolder(out IShellItem ppsi);
            void GetCurrentSelection(out IShellItem ppsi);
            void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
            void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
            void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            void GetResult(out IShellItem ppsi);
            void AddPlace(IShellItem psi, int fdap);
            void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            void Close(int hr);
            void SetClientGuid(ref Guid guid);
            void ClearClientData();
            void SetFilter(IntPtr pFilter);
        }

        [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            void BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, out IntPtr ppv);
            void GetParent(out IShellItem ppsi);
            void GetDisplayName(uint sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
            void Compare(IShellItem psi, uint hint, out int piOrder);
        }

        #endregion
    }
}
