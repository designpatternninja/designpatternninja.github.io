---
layout: page
title: Contribute
permalink: /contribute/
---

![logo.png](logo.png)

# Design pattern ninja catalog

This is a community-driven, open catalog of software design patterns. 

Its main goal is cataloging as many design patterns as possible, and not only the most famous ones, but it's a place where anyone can publish new and invented design patterns and/or improve existing ones.

## What is the goal of this project?

We already have many design pattern sources on the net, and we have also many physical books and *ebooks* about the topic done by great software guys and girls.

But we still need a free, democratic, community-driven software design pattern online catalog, because any software guy and girl has his/her own approaches and **we will evolve as even better software crafters if we understand that sharing our knowledge does not defeat the purpose of our love and effort about software, but it will take it to the next level.**

Software is a kind of science, and science has always evolved opening minds, sharing knowledge and avoiding pattents or secrets, and of course, validating the knowledge against the community. We software crafter are not different from that. 

In summary, our goal is turning the *quality software* into an open resource to everyone involved in the software industry and enforcing the rule of *do not create a new wheel if you are going to create a square wheel*.

### How do I browse already contributed design patterns?

It is as easy as browsing the repository itself here in GitHub. They are divided in categories (directories) in the root directory of this repository. Each Markdown file (i.e. `[file name].md`) is a design pattern in the catalog. Open it and GitHub will render an already parsed version of for the design pattern Markdown source.

### Why "ninja"?

Call it ninja, *guru*, *expert*. This is a repository full of design pattern ninjas! ;)

## How do I contribute?

**Everyone can contribute here**. Contributing is easy: you just need to know the basics of GIT source version control system.

1. First of all, you need fork this repository and clone it on your own machine: `git clone https://github.com/designpatternninja/catalog.git`. Also, you will need to own a GitHub user already.
2. Add new design patterns or modify existing ones following the conventions (follow this link to learn more about them).
3. Once you have already done your work, you will need to *commit* it to your local repository: `git commit`. Please, provide a commit title/description that briefly-explains what you are contributing.
4. Now *push* your local repository changes to your remote repository on GitHub: `git push`.
5. Open a pull request to merge your contribution with our master branch.

In the other hand, if you want to discuss how to improve or fix some design pattern, or you want to start a philosophical discussions to criticize or create a new design pattern, we have a very powerful yet easy to use tool: [the GitHub repository's built-in issue tracker](https://github.com/designpatternninja/catalog/issues). Publish issues and we will look forward for your concerns there all together!

## Contribution conventions

* All contributions must be in English language.
* They must be formatted using GitHub-flavored Markdown.
* They must not be patented or copyrighted design patterns or copy-pasted articles. We only accept genuine content. You can still contribute with existing design patterns specifying who is the original creator, but the design pattern explanation must be yours.
* Any contribution must fit well in a single Markdown file, and the whole file name must be the design pattern name (for example: *Service layer.md*).
* Any image or embedded resource must be placed in a directory with the same name as the main Markdown file. Also, we do not accept external resources: all resources should be uploaded to the respository.
* You should provide as many information as possible, but design patterns should be just *defined*: each design pattern should not look like an encyclopedia (we want to avoid readers be on *Too Long, Don't Read* case...).
* All design patterns must use `Template.md` as template. Copy paste its basic content and fill the gaps in your own way. *Remember to copy-paste the Markdown source code, not the rendered Markdown as HTML!*. Note that we also accept improvements to this template using regular *pull request* approach.
* The root directory contains design pattern category. If no directory/category fits your design pattern, just create a new one and during pull request we will discuss if it needs further modifications.
* All contributed content will be available using [Creative Commons Zero license](https://creativecommons.org/publicdomain/zero/1.0/).
* Bibliography or references are optional. But if your work is based on some other, you must reference the source of your work with links.