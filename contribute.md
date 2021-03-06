---
layout: page
title: Contribute
excerpt: If you love software development, you've tons of ideas to share good practices. Contribute to Design Pattern NINJA today! 
permalink: /contribute/
categories: app-menu
order: 3
---

This is a community-driven, open catalog of software design patterns. 

Currently the entire site is built on top of [Jekyll](http://jekyllrb.com).

Our main goal is to catalog as many design patterns and anti-patterns as possible, and not only the most famous ones, but it's a place where anyone can publish new and invented design patterns and/or improve existing ones.

## What is the goal of this project?

We already have many design pattern sources on the net, and we have also many physical books and *ebooks* about the topic done by great software guys and girls.

But we still need a free, democratic, community-driven software design pattern online catalog, because any software guy and girl has his/her own approaches and **we will evolve as even better software crafters if we understand that sharing our knowledge does not defeat the purpose of our love and effort about software, but it will take it to the next level.**

Software is a kind of science, and science has always evolved opening minds, sharing knowledge and avoiding pattents or secrets, and of course, validating the knowledge against the community. We software crafter are not different from that. 

In summary, our goal is turning the *quality software* into an open resource to everyone involved in the software industry and enforcing the rule of *do not create a new wheel if you are going to create a square wheel*.

### Why "ninja"?

*Ninja*, *guru*, *expert*. This is a repository full of design pattern ninjas! ;)

## How do I contribute?

**Everyone can contribute here**. Contributing is easy: you just need to know the basics of GIT source version control system.

1. First of all, you need fork this repository and clone it on your own machine: `git clone https://github.com/[your GitHub user]/designpatternninja.github.io.git`. Also, you will need to own a GitHub user already.
2. Add new design patterns or modify existing ones following the conventions (take any already published design pattern as sample).
3. Once you have already done your work, you will need to *commit* it to your local repository: `git commit`. Please, provide a commit title/description that briefly-explains what you are contributing.
4. Now *push* your local repository changes to your remote repository on GitHub: `git push`.
5. Open a pull request to merge your contribution to our master branch.

**Probably you'll want to preview your collaboration while you compose it**. As DesignPatternNINJA has been built using Jekyll and it's hosted on GitHub Pages, [please follow these steps to install Jekyll on your own machine](https://help.github.com/articles/setting-up-your-github-pages-site-locally-with-jekyll){:target="_blank"}.

In the other hand, if you want to discuss how to improve or fix some design pattern, or you want to start a philosophical discussions to criticize or create a new design pattern, we have a very powerful yet easy to use tool: [the GitHub repository's built-in issue tracker](https://github.com/designpatternninja/designpatternninja.github.io/issues). Publish issues and we will look forward for your concerns there all together!

## What to contribute

In Design Pattern NINJA you can already contribute with:

1. Design patterns and anti-patterns.
2. News articles. 

## Contribution conventions

* All contributions must be in English language.
* They must be formatted using markdown.
* They must not be patented or copyrighted design patterns or copy-pasted articles. We only accept genuine content. You can still contribute with existing design patterns specifying who is the original creator, but the design pattern explanation must be yours.
* Any contribution must fit well in a single markdown file, and the whole file name must be the design pattern name prefixed and it must live within `./catalog/design-patterns`, `./catalog/anti-patterns` or `./_news`  repository directories.
* Any image must be placed in a directory within `./img` with the same name as the main markdown. When we talk about news articles, they should be placed within `./img/news` article. News article's headers must be within `./img/news/headers` where the header image file must be the slugified article's title in JPG format. Design patterns and anti-patterns images must be placed in the root `./img` directory, creating subdirectories where their names will be the whole patterns' names.
* You should provide as many information as possible, but design patterns should be just *defined*: each design pattern should not look like an encyclopedia (we want to avoid readers be on *Too Long, Don't Read* case...).
* All contributed content will be available using [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International license](http://creativecommons.org/licenses/by-nc-sa/4.0/).
* Bibliography or references are optional. But if your work is based on some other one, you must reference the source of your work with links.