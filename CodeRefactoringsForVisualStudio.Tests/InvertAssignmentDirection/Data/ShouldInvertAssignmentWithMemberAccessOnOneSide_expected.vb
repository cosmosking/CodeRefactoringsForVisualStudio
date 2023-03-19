Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace CodeRefactoringsForVisualStudio.Tests.InvertAssignmentDirection.Data
    Class Foo
        Public FiledMember As Integer
        Public Property PropertyMember As Integer
    End Class

    Class ShouldInvertAssignmentWithMemberAccessOnOneSide
        Private Sub Foo()
            Dim foo = New Foo()
            Dim test As Integer = 666
            test = foo.PropertyMember
        End Sub
    End Class
End Namespace
