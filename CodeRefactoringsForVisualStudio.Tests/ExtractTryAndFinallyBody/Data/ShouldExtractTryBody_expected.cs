using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRefactoringsForVisualStudio.Test.ExtractTryAndFinallyBody.Data
{
    class ShouldExtractTryAndFinallyBody
    {
        class Foo
        {
            public int FiledMember;
            public int PropertyMember { get; set; }
        }


        void Try(Foo foo1, Foo foo2)
        {
            {
                foo2.PropertyMember = foo1.PropertyMember;
                foo2.FiledMember = foo1.FiledMember;
            }
        }
    }
}
