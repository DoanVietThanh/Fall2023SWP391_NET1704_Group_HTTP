import React from "react";
import Header from "../../components/Header";
import Footer from "../../components/Footer";
import BackgroundSlider from "../../components/BackgroundSlider";

const DocumentPage = () => {
  const url =
    "https://themeholy.com/wordpress/edura/wp-content/uploads/2023/07/breadcumb-bg.png";
  const breadcrumbs = "Document";
  return (
    <div>
      <Header/>
      <BackgroundSlider url={url} breadcrumbs={breadcrumbs} />
      <Footer/>
    </div>
  );
};

export default DocumentPage;
