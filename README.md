# NurseServerApp

👩‍💻 Универсальная система приема, обработки и отправки входящих сообщений от пользователей сайта в телеграм-чат администратору

## Проблематика
На данный момент не существует удобных и оптимальных способов получения сообщений от пользователей сайта в мессенджер. Обычно разрабатываются отдельные админ-панели в базах данных которых и хранятся сообщения/заявки от пользователей, но для простых сайтов-визиток такой подход очень избыточен и не оптимален

## Задачи

♨️ разработка модуля API для получение сообщейний/заявок с сайта

 📂 разработка и подключение отдельного сервиса, который выполнял роль двустороннего TCP-сервера

 🔗 создание модуля телеграмм-бота для получения сообщений с API сервера и их пересылка в чат

🧂 разработка функционала по авторизации администратора сайта, который будет получать сообщения от пользователей сайта

 🐕‍🦺 разработка функционала подключению сервера API к клиенту в виде модуля телеграм-бота

🧪 сбор тестовых данных

🤖 разработка модуля приема сообщений от пользователей не только с сайта, но и самого телеграм-бота

🏃 разработка дополнительного функционала

## Объекты

👨 пользователь сайта

💼 пользователь телеграм-бота

⚖️ администратор


## Функционал

Основной функционал:
- ограничение по приему сообщений от одного пользователя;
- прием сообщейний от пользователя сайта через API;
- авторизация администратора через JWT;
- авторизация пользователя сайта через JWT

Дополнительный функционал:
- просмотр всех сообщений/заявок от пользователя сайта в чате;
- приверка подлиности API сервера;
- приверка подлиности телеграм-бот сервера;
- создание TCP/IP соединение между API сервером и телеграм-бот сервером;
- двусторонние общение между API сервером и телеграм-бот сервером;
- просмотр всех сообщений/заявок от пользователя телеграм-бота в чате;
- автоматическое подключение телеграм-бот сервера к API серверу;
  
и другие функции

## Особенности

Система может работать в локальной сети или на удаленном сервере, но при этом для ее работы необходим постоянный доступ к Интернету. Сама система не требует для хранения заявок базы данных т.к. сам чат в мессенджере и является базой данных для хранения сообщений/заявок

## Технологический стек

Сервер: ASP.Net Core API, .Net Core 7

Сервер базы данных: -

Клиент: ASP.Net Core API, .Net Core 7

Клиент/Телеграм бот: ConsoleApp .Net Core 7

API разработано с подходом Code First

API разработано по паттерну MVC

## Тесты

--

## Локальный запуск

Для запуска системы необходимо сначала запустить сервер API

Запуск API

```dotnet run```

Далее необходимо запустить консольное приложение телеграм бота на .Net Core и затем проверить его работоспособность.

Перед этим необходимо настроить файл в папке Data -  config.json и указать адрес API сервера и настроить файл tgtoken.txt указав токен телеграм бота.

Запуск приложения бота

```dotnet run```

В итоге, систему  можно запустить на одном или нескольких локальных серверах или удаленных, модули системы можно запустить на разных серверах, но при этом они должны быть связаны друг с другом

## Автор
- [@volodimirln GitHub](https://github.com/volodimirln)
- [@volodimirln Vk](https://vk.com/volodimirln)
- [@volodimirln Tg](https://t.me/volodimirln)

## Лицензия

[MIT](https://choosealicense.com/licenses/mit/)


## Демонстрация

--

## Деплой

Развертывание происходит только со стороны сервера API и телеграм-бот сервера, серверное ПО может быть развернуто на разных устройствах

## Приложение

Подробное описание проекта представлено в личном телеграм канале


Разработано в рамках коммерческого проекта


## Значки

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
