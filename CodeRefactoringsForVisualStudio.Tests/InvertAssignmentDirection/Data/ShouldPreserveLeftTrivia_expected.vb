﻿Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace CodeRefactoringsForVisualStudio.Test.InvertAssignmentDirection.Data
    Class ShouldPreserveLeftTrivia
        Public Sub Foo(ByVal a As Integer, ByVal b As Integer)
left:       b = a
        End Sub
    End Class
End Namespace
