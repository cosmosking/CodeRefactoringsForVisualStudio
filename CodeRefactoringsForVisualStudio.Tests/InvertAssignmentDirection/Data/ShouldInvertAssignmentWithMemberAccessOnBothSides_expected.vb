Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace CodeRefactoringsForVisualStudio.Test.InvertAssignmentDirection.Data
    Class ShouldInvertAssignmentForMemberAccess
        Class Foo
            Public FiledMember As Integer
            Public Property PropertyMember As Integer
        End Class

        Private Sub Change(ByVal foo1 As Foo, ByVal foo2 As Foo)
            foo2.FiledMember = foo1.PropertyMember
            foo1.PropertyMember = foo2.FiledMember
            foo2.PropertyMember = foo1.PropertyMember
            foo2.FiledMember = foo1.FiledMember
        End Sub
    End Class
End Namespace
