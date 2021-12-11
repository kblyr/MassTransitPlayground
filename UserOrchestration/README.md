# UserOrchestration

Here, I try to be familiarized with **MassTransit**'s _State Machine_, how it works and how to work with it and using it together with other frameworks and libraries.

I am going to create an orchestration that handles the life-cycle of a _User_ of the system.
* Create User
* Email Verification
* Activation/Deactivation

## Core
This is where you can find all the messages/contracts, system-wide services and general-purpose utilities.

## EntityFrameworkCore
The persistence layer that uses _Entity Framework Core_, primarily for _commands_ and _basic queries_

## UserEmailOrchestratorService
A background worker service that is used to handle the state of **Email Verification**

## UserOrchestratorService
A background worker service that is used to handle the state of **User**

## WebApi
A resource endpoint interface for the client.

By design, it should be the only part of the system that is publicly available (not including the front-end UI of course)