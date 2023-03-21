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
            [|try
            {
                foo2.PropertyMember = foo1.PropertyMember;
                foo2.FiledMember = foo1.FiledMember;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                foo2.PropertyMember = foo1.PropertyMember;
            }|]
        }
    }
}
