using System.Collections;
using System.Collections.Generic;
using System;

public static class BookEvents
{
    public static event EventHandler CloseBook;

    public static void CloseBookFunction()
    {
        if (CloseBook != null)
            CloseBook(new object(), new EventArgs());
    }
}
