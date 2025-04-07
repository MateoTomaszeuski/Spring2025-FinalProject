<div style="text-align: right"> Date </div>

# Weekly Report X

## Progress Summary
### Scope Progress
Progress on scope (show what you've done, what remains)

### Learning Objectives Progress
Progress on learning objectives (note which objectives you can demonstrate through this project, which you cannot)

---

## Work Logs
These time logs outline our meetings spent mob programming, debugging, and planning as a group.

[Insert Time Logs Here]

---
## Blocking Issues
Share 3+ different blocking issues that you were stuck on for a while, and how you resolved them.

#### 1. Multiple Font Family Incompatibility in MAUI Button
**Description** 
  - On the To-Do page, I added the ability to sort a list by category in both directions (both ascending and descending). To make this clear in the UI, I wanted the "Sort by Category" button to also have an icon that dynamically changes from an up/down arrow depending on the direction of the sort. The button's text property only supports one font family, but I'm using FontAwesome for icons and needed to set that accordingly. I didn't know how to include both text (with the default FontFamily) and the icon (with the FontAwesome FontFamily) in the same button element.

**Attempts To Fix:**
  - I thought about trying to overlay elements so they looked like a single element, but this felt like it would be prone to misfunctioning, especially in the mobile version of the app.
  - I realized I could create my own button and try to make it match the default button, but I had to do some research on using GestureRecognizers in the Grid.
 
**Solution:**
  - I basically created my own button from scratch using a border, a grid, and a gesture recognizer. This solution led to a lot of XAML code, but it does look the way I wanted it to.

#### 2. ViewModel Breaks Conditionally Visible Tools
* **Description:** 
  - We're using custom controls to render the individual tools on the tools page. Having ViewModels for each of the tools seems to break the Tools page.
* **Solution:**
  - Workaround: use the code-behind for the tools logic instead of MVVM.

#### 3. Issue
* **Description:** 
  - lorem
* **Attempts To Fix:**
  - lorem 
* **Solution:**
  - lorem
