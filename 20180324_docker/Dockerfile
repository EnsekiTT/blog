FROM python:3.6.4-alpine

ARG project_dir=/web/myweb/

ADD requirements.txt $project_dir
ADD docker.py $project_dir

WORKDIR $project_dir
RUN pip install -r requirements.txt

RUN apk update
RUN apk add zsh vim tmux git tig

