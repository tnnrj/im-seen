# IMseen
A robust and highly customizable reporting dashboard built to track and analyze crowdsourced event data. 

Created by request of [the IM Foundation](https://www.theimfoundation.org/) to allow mental health experts in schools to receive community and staff observations about their students' behavior. Built at the University of Utah as part of the 2021 Graduates' Capstone.

![Sample Dashboard](https://user-images.githubusercontent.com/32319011/154618352-620dff37-674d-4038-9120-e41d93e7a234.png)

## Mental Health Reporting

In the U.S., the suicide and depression rate of K-12 students is high. To help prevent these tragedies, we need a way to detect indicators of a student in crisis (i.e. worrying behaviors, expressions, performances) so that school faculty can assess if that student needs help and to what degree. All staff members are in a position to notice events where they think a student may need attention or intervention. Our stakeholders need a way to monitor alarming behavior in real-time and identify potentially troubled students and their level of concern in a timely manner. 

### Our Solution

- Try to find at-risk students before it becomes a crisis.
- The main idea is to crowdsource information about potential clues about someone’s mental/emotional wellbeing before escalation.
- Our app intends to keep counselors aware of which students may be at risk in hopes of preventing crises.

### How it works

- School staff and community members observe students under their care and record any troubling observations in our [companion mobile application](https://github.com/t-nn-rj/im-seen-mobile)
- Wellness center staff or school counselors track these observations over time on the web dashboard
- These professionals make use of computed "level of concern" in the dashboard to identify at-risk students
- Actions can be planned and outcomes recorded in the portal for future analysis

## Deployment

A docker-compose config and associated `Dockerfile` has been included to ease deployment to Docker platforms on any host. An `nginx` reverse proxy is also necessary to route API traffic.


![Student View](https://user-images.githubusercontent.com/32319011/154618480-ff2b8f58-61f5-42a9-988c-1981758855d9.png)

![Observation View](https://user-images.githubusercontent.com/32319011/154618483-a5a1f817-a345-4599-8f4d-7ba4d21a3cb9.png)

![Dashboard Configuration](https://user-images.githubusercontent.com/32319011/154618489-6c58741b-a8e5-4b90-acfe-22595d60ac35.png)
