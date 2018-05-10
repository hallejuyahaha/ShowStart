from selenium import webdriver
from scrapy.selector import  Selector


# browser = webdriver.Chrome(executable_path="E:\chromedriver\chromedriver.exe")
#
# browser.get("https://www.showstart.com/event/51086")
# print(browser.page_source)
#
# t_selector = Selector(text=browser.page_source)
# a = t_selector.xpath("showname","//div[@class='items ll']/h1[@class='goods-name']/text()[last()]").extract_first("")
# print(a)
# browser.quit()


#设置chrome不加载图片
chrome_opt = webdriver.ChromeOptions()
prefs = {"profile.managed_default_content_settings.images":2}
chrome_opt.add_experimental_option("prefs",prefs)

browser = webdriver.Chrome(executable_path="E:\chromedriver\chromedriver.exe",chrome_options=chrome_opt)
browser.get("https://www.showstart.com/event/51086")














