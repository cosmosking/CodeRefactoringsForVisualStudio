﻿Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace CodeRefactoringsForVisualStudio.Test.InvertAssignmentDirection.Data
    Class C
        Private Sub M()
            Dim i As Integer = 1
            Dim j As Integer = 2
            j = i
            i = j
        End Sub
    End Class
End Namespace
