# OneWayMail

A Trash Mail Service for your own mail domain.

OneWayMail is an online service to generate temporary e-mail addresses with forwarding to your personal mailbox.

## What's this project about?

Most mailservers support setting up a so-called catch-all mail forwarding to a single mailbox. That's where all emails to ...@yourmaildomain would end up. The service does nothing but monitor the mailbox and then forward the emails depending on what rules you have set via the web ui.

![image](https://user-images.githubusercontent.com/20394732/135268017-5b774220-059f-4ce1-bb4a-7e37758bd1ec.png)

## Prerequisites

* An own mail domain
* Mail server (I love hMailServer) which supports IMAP.
* A catch-all mailbox
* Windows Server with IIS
* .NET 4.8 Framework
* SQL Server

## Configuration

OneWayMail has following components:
* A **Web UI** where users can request new e-mail-adresses.
* A **Windows Service** that constantly monitors the Catch-All mailbox for new mails and forwards the mails using the forwarding rules if necessary.
* The forwarding rules and logs are stored in a **SQL Server database.**

The Web UI and the Windows Service share the same SQL Server database.

### Web UI

You have to configure the connection string to the database [here](./Doering.OneWayMail.Web/Web.config)

### Service

The connection string and the IMAP/SMTP settings must be configured [here](./Doering.OneWayMail.Service/App.config)
