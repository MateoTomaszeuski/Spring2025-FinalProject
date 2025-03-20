
<div style="text-align: right"> 03/22/2025 </div>

# Weekly Report 1

## Progress Summary

#### Scope Progress
Progress on scope (show what you've done, what remains)

#### Learning Objectives Progress
Progress on learning objectives (note which objectives you can demonstrate through this project, which you cannot)

---

## Work Logs
These time logs outline our meetings spent mob programming, debugging, and planning as a group.

[Insert Time Logs Here]

---
## Blocking Issues
Share 3+ different blocking issues that you were stuck on for a while, and how you resolved them.

#### 1. Static Site Deployment - npm
* **Description:** 
  - npm is breaking only within our folder.
* **Attempts To Fix:**
  - Tried pulling the docker image found in the workflow, then exec-ed into the container and looked at the relevant script. It didn't tell us anything helpful
  - Escalated to Alex, then solved the problem on our own. 
* **Solution:**
  - We read the error messages in the deployment logs more clearly, and realized that the output location was incorrect. Adjusted this to work with vite, and fixed the problem.

#### 2. Static Site Not Building
* **Description:** 
    - npm is broken in our project folder, but we can work around this by using vite commands directly:
      - `Vite`
      - `Vite Build`