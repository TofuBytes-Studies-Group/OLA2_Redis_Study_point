# OLA2_Redis_Study_point

# Redis Integration Report – CRUD Operations via C# & Swagger

## Overview
**Group Members: Jamie & Isak**
The goal of this implementation was to set up a Redis-based data store and build a simple C# application using Swagger for interaction. The focus was to enable full CRUD (Create, Read, Update, Delete) functionality while adhering to a basic security and expiration policy for stored user data.

We selected these Redis Configurations:
- 1 - Redis with Retention Policy 
- 4 - Redis Security
As these were expected to integrate nicely with each other. 

---

## Implementation Steps

We began by spinning up a Redis instance locally and configuring ACL (Access Control Lists) to create isolated user access. A user named `myuser2` was assigned full privileges (`+@all`) with a hashed password for security. Additional users were created for experimentation, including a read-only user with limited permissions called **readonlyuser**
![[Pasted image 20250410130034.png]]
**Altough** as seen in the above screenshot with the **"24hourstolive"** users, we initially did make use of some very confusing naming conventions that most likely isn't best practice at all but that said we did improve upon that in the later examples.

Next, in the C# project, we:

1. Integrated the `StackExchange.Redis` library for communication with Redis.
2. Exposed endpoints via an ASP.NET Core Web API project.
3. Configured Swagger to display our API and allow direct interaction.
4. Ensured all keys related to user data were prefixed with `user:24hourstolive:*`, and added TTLs of 24 hours to them for automatic expiry.

The TTL was enforced at the time of data insertion using `SETEX` or `SET` followed by `EXPIRE`. This guaranteed automatic cleanup after 24 hours, aligning with our use case of temporary user data.
![[Pasted image 20250410130355.png]]

---

## Challenges & Learnings

The most significant challenge was around **user authentication in Redis**, Redis does not support logging in with unhashed passwords when users are configured using the `ACL SETUSER` command with a hashed password. This created confusion and led to several failed attempts before resolving the issue by using the correct pre-hashed password in the CLI command.
Altough this didn't come as a surprise to us, that it wasn't possible. But after long hours of working on this we became somewhat blind to the obvious and didn't realize we just should've used the hashed password the first time around.

Another frustrating aspect was the error messaging when trying to access Redis from an external source. Redis runs in "protected mode" by default and rejects outside connections unless:

- A password is set for the default user, or
- Protected mode is explicitly disabled.

We opted to **set a password for the default user**, which allowed external connections while keeping the instance relatively secure. Mostly as it felt Unsafe to disabled protected mode and we imagined it being unadvisable even tough Redis itself isn't known to be safe anyways.

Another discovery was that **Redis doesn’t support querying keys like `GET user`** since keys are purely string-based and don’t have types like “users.” Keys had to be manually scanned using `SCAN` with a matching pattern like `user:24hourstolive*`, and TTLs had to be individually fetched using `TTL keyname`.
![[Pasted image 20250410131015.png]]

Swagger integration was relatively smooth though.

---

## Conclusion

Despite the hiccups with Redis ACLs and key scanning limitations, we now have a fully functional Redis-backed API with proper security and time-bound data expiration. The process highlighted both the power and simplicity of Redis.

Lastly here is a PDF of the working API endpoints: https://github.com/TofuBytes-Studies-Group/OLA2_Redis_Study_point/blob/main/OLA2_Redis_APP_ENDPOINTS.pdf 

---

