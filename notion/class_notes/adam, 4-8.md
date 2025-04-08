# Tidying

Commits should have a single type of change. 
If you made multiple changes, you can stage singular files with 
`git add file/path/name.txt`, then `git commit -m "message"`. This allows you 
to break your commits into smaller bites to be reverted from instead of larger 
commits that are harder. 

We refactored the JSONConfigFile class in this [github repo](https://github.com/Cody-Howell/ConfigFileLibrary)
(it is my library). We refactored the constructor to call a single private method that returned an 
`IBaseConfigOption` value, which looked a bit cleaner. 

The majority of our time was spent talking about the Tidying rules and applying them to our code. 
We refactored the ReadAsList method, using a little bit of extracting-till-you-drop, and otherwise 
generally figuring out what the code was intending to do. 
