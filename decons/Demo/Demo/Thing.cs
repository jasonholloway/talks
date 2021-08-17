using System;
using Demo;

var thing = new Thing("name");

var blah = new
{
    Name = "Bruce"
};

switch (thing)
{
    case Thing { Name: var n }:
        throw new NotImplementedException();
}

