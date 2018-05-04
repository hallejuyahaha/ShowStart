# -*- coding: utf-8 -*-

# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://doc.scrapy.org/en/latest/topics/item-pipeline.html
import MySQLdb
import MySQLdb.cursors

class ShowstartPipeline(object):
    def process_item(self, item, spider):
        return item


class MysqlPipeline(object):
    #采用同步的机制写入mysql
    def __init__(self):
        self.conn = MySQLdb.connect('127.0.0.1', 'root', 'Abcd1234', 'test', charset="utf8", use_unicode=True)
        self.cursor = self.conn.cursor()

    def process_item(self, item, spider):
        insert_sql = """
            insert into showstart(showname, actor, price, time,place,url,type)
            VALUES (%s, %s, %s, %s, %s, %s, %s) 
        """
        self.cursor.execute(insert_sql, (item["showname"], item["actor"], item["price"],  item["time"],  item["place"],  item["url"],  item["type"]))
        self.conn.commit()