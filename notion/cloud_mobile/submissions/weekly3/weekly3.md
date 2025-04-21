<div style="text-align: right"> 4/5/2025 </div>

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

#### 1. MAUI Picker Binding in Collection View
* **Description:** 
  - Selecting an item in the picker, then sorting tasks by category, led to the category values on some of the items becoming null.
* **Attempts To Fix:**
  - We spent almost all of class time on Monday trying to troubleshoot this.
* **Solution:**
  - No straightforward solution, so we found a workaround. Now, the collection is set when the item is created, and cannot be modified once the item has been added to the collection view.

#### 2. App functionality: allowing multiple lists
* **Description:** 
  - Our group spent some time discussing the pros and cons of allowing users to have multiple lists. As this decision would have a significant impact on the design of the database and relevant services, we didn't want to move forward until we had come to a solid decision.
* **Attempts To Fix:**
  - We ultimately decided that having multiple lists adds unnecessary complexity, especially considering that users already have the ability to categorize items in their list. 
* **Solution:**
  - We decided to adjust our current design to accommodate one list per user. We will also likely add a "filter" feature within the to-do page to more clearly sort by category.

#### 3. Binding 'enter' key to a MAUI entry
* **Description:** 
  - Wanted to set up the To-Do view to allow users to press 'enter' on the keyboard to add a task, but didn't know whether this would be a responsibility of the ViewModel or the code-behind.
* **Attempts To Fix:**
  - Some google searches pointed me in the right direction. 
* **Solution:**
  - Defined an event handler for adding both tasks and subtasks. The MAUI entry has a "Completed" property that can be assigned event handlers like these.