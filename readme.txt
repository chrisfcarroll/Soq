Soq is intended as an extremely simple solution to the problem of dependencies when unit testing code.
It implements the <a href="http://www.cafe-encounter.net/p157/simple-object-quotation">simple object pattern</a>

This is a C# implementation. It should take you all of 2 or 3 minutes to rewrite it entirely in your own favourite language. If you do, feel free to post in the comments on the home page at http://www.cafe-encounter.net/p157/simple-object-quotation.

It is perhaps a stretch to call object quotation a pattern; the pattern is kind of a service locator. Except it's a service locator that is so simple it will only locate what you tell it.

You can immediately see when not to use it: if you require real runtime configurability then object quotation is not your answer.