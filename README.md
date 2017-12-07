# OneWayMail

A Trash Mail Service for your own mail domain.

OneWayMail is an online service to generate temporary e-mail addresses with forwarding to your personal mailbox.

## Prerequisites

* An own mail domain
* Mail server (I love hMailServer) which supports IMAP.
* A catch-all mailbox
* Windows Server with IIS
* .NET 4.5.2 Framework
* SQL Server

OneWayMail has following components:
* A **Website** where users can request new e-mail-adresses.
* A **Windows Service** that constantly monitors the Catch-All mailbox for new mails and forwards the mails using the forwarding rules if necessary.
* The forwarding rules and logs are stored in a **SQL Server database.**