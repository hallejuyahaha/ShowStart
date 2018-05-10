from scrapy.cmdline import execute

import  sys
import  os
import datetime


print(os.path.dirname(os.path.abspath(__file__)))
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

# while True:
#     now = datetime.datetime.now()
#     if now.hour == h and now.minute == m:
execute(["scrapy","crawl","ShowStartSpider"])
    # time.sleep(60)
