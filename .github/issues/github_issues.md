# Lexicon LMS — GitHub Issues / Product Backlog

## 🏃 Sprint 1 (~73 points) — Core Auth & Course Structure

### [US-01] User Registration
**As a** student or instructor,
**I want to** register with email and password,
**So that** I can access the platform.

**Acceptance Criteria:**
- [ ] Email and password are required fields
- [ ] Email must be unique in the system
- [ ] Password minimum 8 characters, must include a number
- [ ] Confirmation email sent on successful registration
- [ ] Validation errors shown inline

**Labels:** `backlog`, `sprint-1`, `authentication`, `user-story`
**Story Points:** 5

---

### [US-02] User Login
**As a** registered user,
**I want to** log in with my credentials,
**So that** I can access my dashboard.

**Acceptance Criteria:**
- [ ] Login with email and password
- [ ] Redirect to role-based dashboard after login
- [ ] Show error on invalid credentials
- [ ] "Remember me" checkbox supported
- [ ] Lockout after 5 failed attempts

**Labels:** `backlog`, `sprint-1`, `authentication`, `user-story`
**Story Points:** 5

---

### [US-03] User Logout
**As a** logged-in user,
**I want to** log out securely,
**So that** my session is ended safely.

**Acceptance Criteria:**
- [ ] Logout button available on all pages
- [ ] Session is cleared on logout
- [ ] Redirected to login page after logout
- [ ] Cannot access protected pages after logout

**Labels:** `backlog`, `sprint-1`, `authentication`, `user-story`
**Story Points:** 2

---

### [US-04] Role Assignment (Student / Instructor / Admin)
**As an** admin,
**I want to** assign roles to users,
**So that** permissions are correctly applied.

**Acceptance Criteria:**
- [ ] Three roles: Student, Instructor, Admin
- [ ] Admin can change any user's role
- [ ] Role is displayed on user profile
- [ ] Role-based navigation menu shown

**Labels:** `backlog`, `sprint-1`, `authentication`, `user-story`
**Story Points:** 8

---

### [US-05] View User Profile
**As a** user,
**I want to** view and edit my profile,
**So that** my information is up to date.

**Acceptance Criteria:**
- [ ] Profile shows name, email, role, avatar
- [ ] User can update name and avatar
- [ ] Changes saved with success message
- [ ] Email change requires re-verification

**Labels:** `backlog`, `sprint-1`, `user-story`
**Story Points:** 5

---

### [US-06] Create a Course
**As an** instructor,
**I want to** create a new course,
**So that** students can enroll and learn.

**Acceptance Criteria:**
- [ ] Fields: title, description, category, thumbnail
- [ ] Course saved as Draft by default
- [ ] Instructor can publish or unpublish the course
- [ ] Course appears in course catalog when published

**Labels:** `backlog`, `sprint-1`, `course-management`, `user-story`
**Story Points:** 8

---

### [US-07] Edit and Delete a Course
**As an** instructor,
**I want to** edit or delete my courses,
**So that** I can keep content up to date.

**Acceptance Criteria:**
- [ ] Instructor can edit all course fields
- [ ] Delete requires confirmation dialog
- [ ] Cannot delete course with active enrollments
- [ ] Changes reflected immediately in catalog

**Labels:** `backlog`, `sprint-1`, `course-management`, `user-story`
**Story Points:** 5

---

### [US-08] Browse Course Catalog
**As a** student,
**I want to** browse available courses,
**So that** I can find courses to enroll in.

**Acceptance Criteria:**
- [ ] List of all published courses shown
- [ ] Filter by category and keyword search
- [ ] Each card shows title, instructor, thumbnail
- [ ] Pagination or infinite scroll supported

**Labels:** `backlog`, `sprint-1`, `course-management`, `user-story`
**Story Points:** 8

---

### [US-09] Enroll in a Course
**As a** student,
**I want to** enroll in a course,
**So that** I can start learning.

**Acceptance Criteria:**
- [ ] Enroll button on course detail page
- [ ] Confirmation message on successful enrollment
- [ ] Course appears in My Courses after enrollment
- [ ] Cannot enroll in same course twice

**Labels:** `backlog`, `sprint-1`, `enrollment`, `user-story`
**Story Points:** 8

---

### [US-10] View My Enrolled Courses
**As a** student,
**I want to** see all courses I'm enrolled in,
**So that** I can track my learning.

**Acceptance Criteria:**
- [ ] Dashboard shows enrolled courses
- [ ] Shows progress percentage per course
- [ ] Quick link to continue last lesson
- [ ] Sorted by last accessed

**Labels:** `backlog`, `sprint-1`, `enrollment`, `user-story`
**Story Points:** 5

---

## 🏃 Sprint 2 (~37 points) — Lessons, Assignments & Grading

### [US-11] Add Lessons to a Course
**As an** instructor,
**I want to** add lessons to my course,
**So that** students have structured content to follow.

**Acceptance Criteria:**
- [ ] Lessons have title, content (rich text), order number
- [ ] Instructor can reorder lessons via drag-and-drop
- [ ] Support for video URL embedding
- [ ] Lesson saved as Draft or Published

**Labels:** `backlog`, `sprint-2`, `lesson-management`, `user-story`
**Story Points:** 8

---

### [US-12] View Lesson Content
**As a** student,
**I want to** view lesson content,
**So that** I can learn the material.

**Acceptance Criteria:**
- [ ] Lessons displayed in order
- [ ] Mark lesson as complete button
- [ ] Navigation to next/previous lesson
- [ ] Progress bar updated after completion

**Labels:** `backlog`, `sprint-2`, `lesson-management`, `user-story`
**Story Points:** 5

---

### [US-13] Track Lesson Progress
**As a** student,
**I want to** track which lessons I have completed,
**So that** I can see how far I've progressed.

**Acceptance Criteria:**
- [ ] Completed lessons marked with checkmark
- [ ] Overall course progress shown as percentage
- [ ] Progress persisted across sessions
- [ ] Progress visible to instructor

**Labels:** `backlog`, `sprint-2`, `progress-tracking`, `user-story`
**Story Points:** 5

---

### [US-14] Create Assignments
**As an** instructor,
**I want to** create assignments for a course,
**So that** I can assess student understanding.

**Acceptance Criteria:**
- [ ] Assignment has title, description, due date, max score
- [ ] Linked to a specific course
- [ ] Students notified when assignment is created
- [ ] Instructor can edit or delete assignment

**Labels:** `backlog`, `sprint-2`, `assignments`, `user-story`
**Story Points:** 8

---

### [US-15] Submit Assignment
**As a** student,
**I want to** submit my assignment,
**So that** my instructor can review it.

**Acceptance Criteria:**
- [ ] Text submission and/or file upload supported
- [ ] Cannot submit after due date
- [ ] Submission timestamp recorded
- [ ] Confirmation message shown after submission

**Labels:** `backlog`, `sprint-2`, `assignments`, `user-story`
**Story Points:** 5

---

### [US-16] Grade Assignment Submissions
**As an** instructor,
**I want to** grade student submissions,
**So that** students receive feedback and a score.

**Acceptance Criteria:**
- [ ] View all submissions per assignment
- [ ] Enter numeric score and written feedback
- [ ] Grade saved and visible to student
- [ ] Average score shown in instructor dashboard

**Labels:** `backlog`, `sprint-2`, `grading`, `user-story`
**Story Points:** 8

---

### [US-17] View Grades
**As a** student,
**I want to** view my grades,
**So that** I can track my academic performance.

**Acceptance Criteria:**
- [ ] Grades page shows all graded assignments
- [ ] Shows score, max score, and feedback
- [ ] GPA or average calculated and displayed
- [ ] Ungraded submissions shown as Pending

**Labels:** `backlog`, `sprint-2`, `grading`, `user-story`
**Story Points:** 5

---

## 📋 Product Backlog (Future Sprints)

### [US-18] Course Completion Certificate
**As a** student,
**I want to** receive a certificate when I complete a course,
**So that** I can showcase my achievement.

**Acceptance Criteria:**
- [ ] Certificate generated when all lessons complete and assignments passed
- [ ] Certificate includes student name, course name, date
- [ ] Downloadable as PDF
- [ ] Shareable link generated

**Labels:** `backlog`, `future`, `certificates`, `user-story`
**Story Points:** 8

---

### [US-19] Discussion Forum per Course
**As a** student or instructor,
**I want to** post and reply in a course discussion forum,
**So that** we can communicate within the course.

**Acceptance Criteria:**
- [ ] Each course has its own forum
- [ ] Students and instructors can post and reply
- [ ] Posts sorted by newest first
- [ ] Instructor can pin or delete posts

**Labels:** `backlog`, `future`, `communication`, `user-story`
**Story Points:** 8

---

### [US-20] Email Notifications
**As a** user,
**I want to** receive email notifications for important events,
**So that** I don't miss updates.

**Acceptance Criteria:**
- [ ] Notify student on new assignment
- [ ] Notify student when assignment is graded
- [ ] Notify instructor on new submission
- [ ] User can opt out from profile settings

**Labels:** `backlog`, `future`, `notifications`, `user-story`
**Story Points:** 5

---

### [US-21] Admin User Management
**As an** admin,
**I want to** view and manage all users,
**So that** I can maintain the platform.

**Acceptance Criteria:**
- [ ] Table of all users with search and filter
- [ ] Admin can activate/deactivate accounts
- [ ] Admin can reset passwords
- [ ] Audit log of admin actions

**Labels:** `backlog`, `future`, `admin`, `user-story`
**Story Points:** 8

---

### [US-22] Admin Course Management
**As an** admin,
**I want to** view and manage all courses,
**So that** I can moderate platform content.

**Acceptance Criteria:**
- [ ] Admin can view all courses regardless of instructor
- [ ] Admin can unpublish any course
- [ ] Admin can delete courses
- [ ] Dashboard shows total courses, enrollments, active users

**Labels:** `backlog`, `future`, `admin`, `user-story`
**Story Points:** 5

---

### [US-23] Search Across Platform
**As a** user,
**I want to** search for courses and lessons,
**So that** I can quickly find what I need.

**Acceptance Criteria:**
- [ ] Global search bar in navigation
- [ ] Results show courses and lessons
- [ ] Results highlight matching keyword
- [ ] No results state handled gracefully

**Labels:** `backlog`, `future`, `search`, `user-story`
**Story Points:** 5

---

### [US-24] Student Dashboard with Analytics
**As a** student,
**I want to** see an overview of my activity and progress,
**So that** I stay motivated and on track.

**Acceptance Criteria:**
- [ ] Shows enrolled courses and completion %
- [ ] Upcoming assignment deadlines
- [ ] Recent grades
- [ ] Total hours spent learning

**Labels:** `backlog`, `future`, `dashboard`, `user-story`
**Story Points:** 5
