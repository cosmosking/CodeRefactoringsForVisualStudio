Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace CodeRefactoringsForVisualStudio.Test.InvertAssignmentDirection.Data
    Class Foo
        Private Sub Fuu(ByVal foo1 As Foo, ByVal foo2 As Foo)
            Dim arry As Integer() = New Integer() {1, 2, 3}
            Dim bary As Integer() = New Integer() {4, 5, 6}
            bary(2) = arry(1)
        End Sub
    End Class
End Namespace
