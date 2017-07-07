Spider = {
    Url = 'https://www.zhihu.com/r/search?q={keyWord}&correction=0&type=content&offset={page}',
    KeyWord = { ".net" },
    CurrentPage = 0,
    Regex = { '<a target="_blank" href="([^"]*)" class="js-title-link">', '"next":"([^"]*)"' },
    Type = "json"
}
